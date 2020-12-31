using Blazorade.Teams.Components.Interop;
using Blazorade.Teams.Components.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazorade.Core.Components;

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
        /// Set to <c>true</c> to have the component take care of authenticating the user.
        /// </summary>
        [Parameter]
        public bool RequireAuthentication { get; set; }

        /// <summary>
        /// The main template for your application.
        /// </summary>
        /// <remarks>
        /// This template is rendered when the application has been properly initialized by this component.
        /// </remarks>
        [Parameter]
        public RenderFragment<ApplicationContext> ApplicationTemplate { get; set; }

        /// <summary>
        /// A template that you can use to provide alternative content in cases when the Teams host is not available.
        /// </summary>
        /// <remarks>
        /// This template is typically rendered when the application is not loaded by Teams.
        /// </remarks>
        [Parameter]
        public RenderFragment HostNotAvailableTemplate { get; set; }

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
        /// Determines whether to show the <see cref="HostNotAvailableTemplate"/> template.
        /// </summary>
        protected bool ShowHostNotAvailableTemplate { get; set; }



        /// <summary>
        /// Controls the rendering of the component.
        /// </summary>
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            
            if(firstRender)
            {
                var isHostAvailable = await this.TeamsInterop.IsTeamsHostAvailableAsync();
                if(isHostAvailable)
                {
                    // Now we know that the app is properly loaded, so we can set the interop module
                    // on the application context for easy access.
                    this.ApplicationContext.TeamsInterop = this.TeamsInterop;
                    try
                    {
                        await this.InitializeAsync();
                    }
                    catch (Exception ex)
                    {
                        await this.TeamsInterop.AppInitialization.NotifyFailureAsync(ex.Message, FailedReason.Other);
                    }
                }
                else
                {
                    this.ShowHostNotAvailableTemplate = true;
                    this.StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Handles the main initialization of the component.
        /// </summary>
        /// <remarks>
        /// This should not be called if the Teams host context is not available.
        /// </remarks>
        private async Task InitializeAsync()
        {

            //---------------------------------------------------------------------------------------
            // The initial initialization. The notify app loaded tells Teams that the application
            // was properly loaded, at least so far. That will remove the loader icon from Teams
            // and reveal the UI of your application.
            // Note, you still have to call the NotifySuccessAsync method at some point to tell
            // Teams that the application loading has completed successfully. Otherwise Teams will
            // show an error screen after some time.
            await this.TeamsInterop.InitializeAsync();
            await this.TeamsInterop.AppInitialization.NotifyAppLoadedAsync();
            //---------------------------------------------------------------------------------------



            //---------------------------------------------------------------------------------------
            // Now well get the context from Teams. When we have it, we'll store it in the
            // application's context and call the StateHasChanged method. This will trigger
            // a rerender of the component, in case the application wants to use the context for
            // some purposes. Not all applications need authentication, you know.
            var context = await this.TeamsInterop.GetContextAsync();
            this.ApplicationContext.Context = context;
            this.StateHasChanged();
            //---------------------------------------------------------------------------------------

            if (this.RequireAuthentication)
            {
                //-----------------------------------------------------------------------------------
                var authResult = await this.TeamsInterop.GetTokenAsync(context);
                this.ApplicationContext.AuthResult = authResult;
                await this.TeamsInterop.AppInitialization.NotifySuccessAsync();

                this.ShowApplicationTemplate = true;
                this.StateHasChanged();
                //-----------------------------------------------------------------------------------
            }
            else
            {
                //-----------------------------------------------------------------------------------
                // If the application does not need authentication, we will notify Teams that the
                // application has successfully loaded. We'll also set the flag that will instruct
                // the UI to render the ApplicationTemplate template and call StateHasChanged to
                // have the component do one more rendering round.
                await this.TeamsInterop.AppInitialization.NotifySuccessAsync();
                this.ShowApplicationTemplate = true;
                this.StateHasChanged();
                //-----------------------------------------------------------------------------------
            }
        }
    }
}
