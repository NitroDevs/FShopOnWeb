# Cursor Background Agent Setup for F# eShop Web

This document explains how to use Cursor's Background Agent feature with the F# eShop Web application.

## What is Cursor Background Agent?

Cursor Background Agents are asynchronous remote agents that can edit and run your code in a remote cloud environment. They allow you to:

- Spawn multiple agents working on different tasks simultaneously
- Run code in isolated cloud environments  
- Automatically install dependencies and run applications
- Monitor agent status and take over control when needed

## Prerequisites

1. **Cursor IDE** with Background Agent feature enabled
2. **GitHub Integration** - Background agents clone repos from GitHub and create branches for their work
3. **Max Mode subscription** - Background agents require Max Mode compatible models

## Environment Configuration

The `.cursor/environment.json` file has been configured for this F# project with:

### Install Command
```bash
dotnet restore && cd src/Microsoft.eShopWeb.Web && npm ci
```
This command:
- Restores .NET NuGet packages for the F# application
- Installs Node.js dependencies for CSS processing

### Terminal Sessions
Two terminal sessions will be automatically started:

1. **Build CSS** - Watches and compiles SCSS files to CSS
   ```bash
   cd src/Microsoft.eShopWeb.Web && npm run start
   ```

2. **Run F# Web App** - Starts the F# web application
   ```bash
   cd src/Microsoft.eShopWeb.Web && dotnet run
   ```

## How to Use Background Agent

### 1. Enable Background Agent
- Open Cursor settings
- Navigate to "Background Agent" section
- Toggle the feature ON

### 2. Launch Background Agent Control Panel
- Press `Ctrl+E` (or `Cmd+E` on Mac) to open the background agent control panel
- This allows you to list agents, spawn new ones, and view their status

### 3. Create a New Agent
1. Click "New Agent" in the control panel
2. Provide a clear task description, for example:
   - "Add a new product category feature to the catalog"
   - "Implement user reviews for products"
   - "Fix the basket calculation bug"
   - "Add dark mode support to the UI"

### 4. Monitor Agent Progress
- Select your agent from the list to view status
- See real-time updates as the agent works
- Enter the remote machine to see the development environment

### 5. Take Over or Send Follow-ups
- Send additional instructions while the agent is working
- Take over control of the remote environment
- Review and merge the agent's changes

## Project Structure Understanding

The background agent will understand this F# project structure:

```
├── src/Microsoft.eShopWeb.Web/
│   ├── Program.fs                 # Main application entry point
│   ├── Domain.fs                  # Domain models
│   ├── Persistence.fs             # Database context
│   ├── Home/                      # Home page components
│   ├── Basket/                    # Shopping basket features
│   ├── Account/                   # User account features
│   ├── Layout/                    # Page layouts
│   └── wwwroot/                   # Static web assets
```

## Technologies in Use

The agent will work with:
- **F# 8.0** with ASP.NET Core
- **Falco** web framework
- **Entity Framework Core** with SQLite
- **Bootstrap 5.3.2** for styling
- **PostCSS/SCSS** for CSS processing

## Security Considerations

- Agents run in isolated AWS VMs
- Your code is stored temporarily for agent execution
- GitHub read-write access is required for repo operations
- Agents auto-run terminal commands (review carefully)
- Enable Privacy Mode for additional security

## Best Practices

1. **Clear Task Descriptions**: Be specific about what you want the agent to accomplish
2. **Review Changes**: Always inspect agent modifications before merging
3. **Use Version Control**: Agents work on separate branches - review PRs carefully
4. **Monitor Resource Usage**: Background agents use Max Mode tokens
5. **Test Agent Work**: Run tests after agent modifications

## Example Agent Tasks

Good tasks for this F# eShop project:

- "Add product search functionality with filters"
- "Implement user authentication with JWT tokens"
- "Add product image upload feature"
- "Create an admin dashboard for managing products"
- "Add unit tests for the basket calculations"
- "Implement product recommendations"
- "Add email notifications for orders"

## Troubleshooting

If the agent fails to start:
1. Check GitHub permissions
2. Verify .NET 8.0 SDK is available in the cloud environment
3. Ensure Node.js 16+ is available for CSS processing
4. Check that SQLite database can be created in App_Data folder

## Getting Help

- **Cursor Documentation**: https://docs.cursor.com/background-agent
- **Discord**: #background-agent channel
- **Email**: background-agent-feedback@cursor.com

Remember: Background agents are powerful but use Max Mode tokens. Monitor your usage and costs accordingly!