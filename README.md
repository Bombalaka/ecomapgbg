# EcoMapGbg

## 🌱 Connecting Gothenburg with reuse spots, second-hand shops, and repair hubs for a more circular city. 🌱

# ♻️ EcoMapGbg (Återbrukskartan)

> A location-based web app for promoting reuse, sharing, and circular consumption in Gothenburg.

  

---

  

## 🌍 Project Overview

  

**EcoMapGbg** helps people in Gothenburg discover and share places where items can be reused instead of thrown away – such as second-hand shops, repair stations, swap shelves, and recycling centers.

  

The project aims to reduce waste and support local sustainability by crowd-sourcing reuse locations and making them easy to find via a searchable map interface.

  

---

  

## 🎯 Key Features

  

- 📍 Interactive map with reuse locations

- 🗂️ Filter by category, neighborhood, opening hours

- ➕ Add new places via form (crowdsourcing)

- 🧾 Detail page with info, opening hours and contact

- 🌱 Future ideas: event calendar, gamification, Västtrafik integration

  

---

  

## 🧪 Tech Stack

| Area          | Technology                |
|---------------|---------------------------|
| Backend       | ASP.NET Core, C#, Clean Code |
| Frontend      | Razor Pages               |
| Database      | MongoDB, Docker           |
| DevOps        | GitHub Actions, Docker    |
| Docs          | Swagger / OpenAPI         |

  

---

  

## 🗂️ Project Structure
```plaintext
EcoMapGbg/                  # ASP.NET Core project root
├── EcoMapGbg.csproj        # Project file
├── Program.cs              # Entry point
├── Components/             # Reusable UI components
│   └── Layout/
│   └── (other .razor files)
├── Pages/                  # Razor pages
│   └── (page files)
├── Controllers/            # MVC Controllers
│   └── LocationsController.cs
├── Data/                   # Data access layer
│   ├── ILocationRepository.cs
│   └── LocationRepository.cs
├── Models/                 # Domain models
│   ├── DTO/
│   ├── Enum/
│   └── Location.cs
├── Properties/
│   └── launchSettings.json
├── Services/               # Business logic/services
│   ├── ILocationService.cs
│   ├── LocationService.cs
│   └── MapHub.cs
├── wwwroot/                # Static frontend files
│   ├── index.html
│   ├── app.js
│   └── style.css
├── .gitignore
├── README.md
├── docker-compose.yml      # For MongoDB setup
└── ecomapgbg.sln           # Solution file

````
## 🚀 Getting Started


### Prerequisites
  

- [.NET 8 SDK](https://dotnet.microsoft.com/)

- [MongoDB](https://www.mongodb.com/) or Docker

- Git

-  Swagger  

---
  
### Clone and run

```bash

# 1. Clone repo

git clone https://github.com/Bombalaka/ecomapgbg.git

cd ecomapgbg

Run `docker-compose up -d` to start MongoDB


# 2. Build 

dotnet build


# 3. Run Project

dotnet run --project EcoMapGbg


````
---

  
## 👥 Contributors

* [Nor](https://github.com/NorAjami) – Project Lead

* [Yotaka](https://github.com/Yotaka88) – Project Lead

  

---
## 🔒 Security & Privacy

- MongoDB connection is protected using environment variables.
- Inputs are validated both client- and server-side.
- No user tracking or ads are used.

## 📄 License


MIT License – feel free to reuse and contribute.

