using Blazorade.Teams.Interop;
using Blazorade.Teams.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazorade.Core.Components;
using System.Diagnostics;
using Blazorade.Msal.Security;

namespace Blazorade.Teams.Components
{
    /// <summary>
    /// The root component for all application pages in your Microsoft Teams tab application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This component inherits from <see cref="BlazoradeComponentBase"/> and uses the
    /// <see cref="BlazoradeComponentBase.ChildContent"/> template property to render content
    /// that is shown while the component initializes your application.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The code sample below shows the basic structure for how you build your application pages using the
    /// <see cref="TeamsApplication"/> component.
    /// </para>
    /// <code>
    /// &lt;TeamsApplication RequireAuthentication="true">
    ///     &lt;ApplicationTemplate Context="ctx">
    ///         &lt;p>Here you will write your application.&lt;/p>
    ///         &lt;p>The user has been properly authenticated as @ctx.AuthResult.IdTokenClaims["name"]&lt;/p>
    ///     &lt;/ApplicationTemplate>
    ///     &lt;HostNotAvailableTemplate>
    ///         Seems that you did not load the app inside of Teams.
    ///     &lt;/HostNotAvailableTemplate>
    ///     &lt;ChildContent>
    ///         Loading...
    ///     &lt;/ChildContent>
    /// &lt;/TeamsApplication>
    /// </code>
    /// </example>
    partial class TeamsApplication
    {

        /// <summary>
        /// Specifies whether to perform authentication and acquire an access token with the default scopes
        /// configured during application startup.
        /// </summary>
        [Parameter]
        public bool RequireDefaultScopes { get; set; }

        /// <summary>
        /// A comma-separated list of scopes to acquire a token for. If <see cref="RequireDefaultScopes"/> is set
        /// to <c>true</c>, then the scopes acquired are combined with the default scopes.
        /// </summary>
        [Parameter]
        public string RequireScopes { get; set; }

        /// <summary>
        /// The main template for your application.
        /// </summary>
        /// <remarks>
        /// This template is rendered when the application has been properly initialized by this component.
        /// </remarks>
        [Parameter]
        public RenderFragment<ApplicationContext> ApplicationTemplate { get; set; }

        /// <summary>
        /// The template that is rendered if initialization fails.
        /// </summary>
        /// <remarks>
        /// The initialization typically fails if the application is loaded outside of Teams,
        /// for instance when running as a standard web application.
        /// </remarks>
        [Parameter]
        public RenderFragment<Exception> InitializationFailedTemplate { get; set; }

        /// <summary>
        /// The template that is displayed while loading the application.
        /// </summary>
        /// <remarks>
        /// Nothing is rendered while loading if this template is not defined.
        /// </remarks>
        [Parameter]
        public RenderFragment<ApplicationContext> LoadingTemplate { get; set; }

        private ApplicationContext _ApplicationContext = new ApplicationContext { };
        /// <summary>
        /// The application context.
        /// </summary>
        /// <remarks>
        /// This context is provided as the context to the <see cref="ApplicationTemplate"/> template.
        /// </remarks>
        public ApplicationContext ApplicationContext
        {
            get => _ApplicationContext;
            set
            {
                if (null == value) throw new ArgumentNullException();
                _ApplicationContext = value;
            }
        }



        /// <summary>
        /// Determines whether to show the <see cref="ApplicationTemplate"/> template.
        /// </summary>
        protected bool ShowApplicationTemplate { get; set; }

        /// <summary>
        /// Determines whether to show the <see cref="ContextNotAvailableTemplate"/> template.
        /// </summary>
        protected bool ShowContextNotAvailableTemplate { get; set; }



        /// <summary>
        /// Controls the rendering of the component.
        /// </summary>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            
            if(firstRender)
            {
                await this.HandleMainProcessAsync();
            }
        }

        private async Task HandleMainProcessAsync()
        {
            try
            {
                if(await this.InitializeAsync())
                {
                    this.StateHasChanged();

                    if (this.RequireDefaultScopes || this.RequireScopes?.Length > 0)
                    {
                        try
                        {
                            await this.HandleAuthenticationAsync();
                        }
                        catch (Exception ex)
                        {
                            await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.AuthFailed);
                        }
                    }

                    this.ShowApplicationTemplate = true;
                }
            }
            catch (Exception ex)
            {
                await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.Other);
            }

            this.StateHasChanged();
        }

        private Exception InitializationException;
        /// <summary>
        /// Handles application initialization.
        /// </summary>
        /// <remarks>
        /// This includes initializing the Teams SDK and notifying that the application was properly 
        /// loaded. After this, other SDK functions can be used. This will also for instance remove 
        /// the loader icon from Teams so that your application can start displaying a UI.
        /// </remarks>
        private async Task<bool> InitializeAsync()
        {
            //---------------------------------------------------------------------------------------
            // First we have to do some basic initialization. This will for instance remove the
            // loading icon from Teams so that the application can start rendering a UI.
            try
            {
                this.InitializationException = null;
                await this.TeamsInterop.InitializeAsync();
                await this.TeamsInterop.AppInitialization.NotifyAppLoadedAsync();
            }
            catch (Exception ex)
            {
                this.InitializationException = ex;
                await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.Timeout);
            }
            //---------------------------------------------------------------------------------------


            if(null == this.InitializationException)
            {
                // If there was an error initializing, there is no point in continuing to set up
                // the application, because everything will fail anyway.


                //-----------------------------------------------------------------------------------
                // Now we'll get the client's time zone offset and store it in the application
                // context so that it is available to code running later on.
                try
                {
                    this.ApplicationContext.ClientTimeZoneOffset = await this.LocalizationService
                        .GetClientTimezoneOffsetAsync();
                }
                catch { }
                //-----------------------------------------------------------------------------------


                //-----------------------------------------------------------------------------------
                // Now we'll get the context from Teams. When we have it, we'll store it in the
                // application's context and call the StateHasChanged method. This will trigger
                // a rerender of the component, in case the application wants to use the context for
                // some purposes. Not all applications need authentication, you know.
                var context = await this.TeamsInterop.GetContextAsync();
                this.ApplicationContext.Context = context;
                this.ApplicationContext.TeamsInterop = this.TeamsInterop;
                //-----------------------------------------------------------------------------------
            }

            return null == this.InitializationException;
        }

        private async Task HandleAuthenticationAsync()
        {
            AuthenticationResult token = null;

            //---------------------------------------------------------------------------------------
            // Construct the requested set of scopes from the default scopes and the additional
            // scopes set on the current component instance.

            var scopes = this.TeamsInterop.Authentication.GetScopes(this.RequireScopes);
            //---------------------------------------------------------------------------------------


            //---------------------------------------------------------------------------------------
            // We use the authentication module here, which will try to acquire the token we want
            // without user interaction, but will fall back to using the authentication dialog
            // if a token could not be acquired.
            try
            {
                token = await this.TeamsInterop.Authentication.AcquireTokenAsync(
                    loginHint: this.ApplicationContext?.Context?.LoginHint, 
                    scopes: scopes
                );
            }
            catch { }
            //---------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------
            // At this point we assume that the token has been resolved, and we set it on the 
            // application context and finish up so that the application itself can be rendered.
            this.ApplicationContext.AuthResult = token;

            await this.TeamsInterop.AppInitialization.NotifySuccessAsync();
            //---------------------------------------------------------------------------------------
        }

    }
}
