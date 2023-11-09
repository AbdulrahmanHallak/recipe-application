# Recipe Application

The Recipe Management Application is a RazorPages platform focused on easy recipe and ingredient management. It leverages ASP.NET Identity for user authentication and authorization. Users can create, edit, and remove recipes and their associated ingredients. The application integrates Google login and email confirmation through the MailGun service for secure access. Additionally, resource-based authorization controls user access to specific recipe-related functionalities.

## Key Features
1. [Authentication using ASP.NET Core Identity](#authentication-using-aspnet-core-identity)
2. [Resource-based Authorization](#resource-base-authorization)
3. [Enabling API endpoints with Resource Filters](#enabling-api-endpoints-with-resource-filters)
4. [Dynamic HTML Element Addition with JavaScript](#dynamic-html-element-addition-with-javascript)
5. [View Component](#view-companents)
6. [CDN-based Static File Serving](#cdn-based-static-file-serving)
7. [Logging with Serilog and writing logs to a structured-logging provider](#logging-with-serilog-and-writing-logs-to-a-structured-logging-provider)


### Authentication using ASP.NET Core Identity

The application harnesses the power of ASP.NET Core Identity and its default UI to create a seamless authentication process for users. Users can easily sign up, log in, and manage their profiles within the system. To enhance user convenience, the integration of Google's API allows for quick registration using Google accounts.

Furthermore, the authentication process is fortified with an additional layer of security through email confirmation. This confirmation functionality is facilitated by the [MailGun Email API service](https://www.mailgun.com/), ensuring that user accounts are verified via email.

![RegisterationForm](/images/RegisterationForm.png)

### Resource-based Authorization

Resource-based authorization is implemented to ensure that only users who have created specific recipes possess the permissions to edit or delete them. This functionality is designed to restrict certain actions based on ownership. For instance, users who are not the creators of a recipe are limited to viewing those recipes.

![Veiwing Recipe When Authenticated](/images/VeiwingRecipeWhenAuthenticated.png)

To enforce this access control, the UI elements associated with edit and delete actions are dynamically hidden or displayed based on the user's authorization level. Moreover, robust server-side checks further fortify the security measures, preventing unauthorized attempts to directly manipulate or access restricted functionalities.

![Veiwing Recipe When unauthenticated](/images/ViewingRecipeWhenUnauthenticated.png)

![Resource based authorization code](/images/ResouceBasedAuthorizationCode.png)


### Enabling API endpoints with Resource Filters

The application introduces a versatile structure by including both controllers and API endpoints. This allows for potential future extensions where the project can serve not only web interfaces but also mobile clients or any other systems that might utilize the exposed API functionalities.

To manage the availability of the API endpoints, a custom resource filter is implemented. This resource filter serves the purpose of enabling or disabling the API, offering flexibility in controlling when the API functionalities are accessible within the application. This design enables a more adaptable and scalable project architecture, ready to serve various client interfaces or integrate with external systems via API endpoints, with the capability to manage their accessibility as needed.

![ApiEnabled ActionFilter](/images/ApiEnabledFilter.png)

### Dynamic HTML Element Addition with JavaScript

The application employs dynamic HTML element addition through JavaScript to facilitate the process of updating or creating recipes.
When users update or create recipes, the dynamic addition of HTML elements permits the addition or removal of ingredients associated with these recipes. Through JavaScript, users can conveniently add new ingredients or remove existing ones, providing a more interactive and user-friendly experience when managing recipe details within the application.

![Recipe Create Form](/images/RecipeCreateForm.png)


### View Companents

The application incorporates ViewComponents, offering a side panel that dynamically adjusts its content based on the current user's authentication status. When authenticated, the panel showcases a list of links to the user's recently created recipes, providing easy access to their own content, enhancing the user experience by conveniently surfacing links to their recipes. Conversely, for unauthenticated users, it provides quick access to the login and registration actions, guiding new or returning users to the required authentication functionalities.

![View Component](/images/ViewComponent.png)

Code:

![View Component Code](/images/ViewComponentCode.png)

### CDN-based Static File Serving

The application optimizes file delivery by serving static files through a CDN, a strategy that is contingent on the application environment (e.g., development, production, etc.). This offers many advantges.

CDNs are known for their globally distributed network, ensuring minimal latencies for file downloads regardless of users' geographical locations. This greatly benefits users accessing the application from different parts of the world, improving download speeds and overall responsiveness.

Additionally, serving files from a CDN alleviates network traffic on the application's servers, reducing the load and allowing for better resource management. Furthermore, since common libraries may already be cached in users' browsers from the CDN, it can lead to quicker access to resources without the need for additional requests. This caching mechanism significantly enhances the application's speed and efficiency, providing a smoother and more responsive user experience.

### Logging with Serilog and writing logs to a structured-logging provider

The application extensively employs Serilog as its primary logging framework. To enhance the logging capabilities, the structured logging provider Seq is integrated as a Docker container hosted on the local machine. This strategic combination enables the generation of structured, comprehensive logs that are both searchable and highly informative.

![Seq Logging](/images/SeqLogging.png)