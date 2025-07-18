# FShopOnWeb

F# take on eShopOnWeb ASP.NET Core sample application.

## Icons, Banners, and Emotes

### Stats

<p>
  <a href="https://github.com/nitrodevs/fshoponweb/graphs/contributors" alt="Contributors">
  <img src="https://img.shields.io/github/contributors/nitrodevs/fshoponweb" /></a>
  <a href="https://github.com/nitrodevs/fshoponweb/stargazers" alt="Stars">
  <img src="https://img.shields.io/github/stars/nitrodevs/fshoponweb" /></a>
  <a href="https://github.com/nitrodevs/fshoponweb/issues" alt="Issues">
  <img src="https://img.shields.io/github/issues/nitrodevs/fshoponweb" /></a>
  <a href="https://github.com/nitrodevs/fshoponweb/blob/master/LICENSE" alt="License">
  <img src="https://img.shields.io/github/license/nitrodevs/fshoponweb" /></a>
</p>

### Contributor Socials

<p>
  <a href="https://twitter.com/intent/follow?screen_name=KyleMcMaster">
    <img src="https://img.shields.io/twitter/follow/KyleMcMaster.svg?label=Follow%20@KyleMcMaster" alt="Follow @KyleMcMaster" />
  </a>
  <a href="https://twitter.com/intent/follow?screen_name=seangwright">
    <img src="https://img.shields.io/twitter/follow/seangwright.svg?label=Follow%20@seangwright" alt="Follow @seangwright" />
</a>  
</p>

## Build / Run

### Application

This project requires two steps to run the application successfully. The first step is to run the `npm: start` VS Code task to compile the SCSS styles. This can be done from VS Code by pressing `ctrl+p`, typing `task`, and selecting `npm: run` from the list. This will run `npm install` before executing. The second step is to run the `dotnet: watch` VS Code task by pressing `ctrl+p`, typing `task`, and selecting `dotnet: watch` from the list. This will run the application which can be accessed at `https://localhost:7055`.

An example of getting started can be shown below:

![Getting Started](https://user-images.githubusercontent.com/11415127/199322147-e693cb4d-669f-4427-80ba-15b862b1ff45.gif)

### Database

Once the application starts (in Development mode) it seeds a SQLite database located at `~\App_Data\FShopOnWeb.sqlite`. The database is recreated every time the application starts.

To interact with this database directly, you can install the VS Code [SQLite](https://marketplace.visualstudio.com/items?itemName=alexcvzz.vscode-sqlite) extension and use it to open the database after it is created, using the `SQLite: Open Database` command from the command palette (`ctrl+shift+p`).

![Open Database](./images/sqlite-vscode-open-database.jpg)

This will add the SQLite Explorer tray under the File Explorer where you can initiate SQL queries or explore the database schema.

![SQLite Explorer](./images/sqlite-vscode-explorer-tray.jpg)

## Related Projects and Inspiration

- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb)
- [Clean Architecture](https://github.com/ardalis/CleanArchitecture)
- [Falco](https://github.com/pimbrouwers/Falco)
- [Typescript Functional Extensions](https://github.com/seangwright/typescript-functional-extensions)
