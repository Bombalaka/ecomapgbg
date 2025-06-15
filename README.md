# EcoMapGbg

## 🌱 Connecting Gothenburg with reuse spots, second-hand shops, and repair hubs for a more circular city. 🌱

  
  

```markdown

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

  

| Area          | Technology              |

|---------------|--------------------------|

| Backend       | ASP.NET Core, C#, Clean Architecture, DDD |

| Frontend      | Razor Pages |

| Database      | MongoDB                  |

| Testing       | xUnit                     |

| DevOps        | GitHub Actions, Docker   |

| Docs          | Swagger / OpenAPI        |

  

---

  

## 🗂️ Project Structure

  

EcoMapGbg/


├── EcoMapGbg/                # Single ASP.NET Core project
│
├── EcoMapGbg.csproj
│
├── Program.cs
│
├── Components/             
│     ├── Layout/
│     ├── Pages/
│     └──  .razor
│
├── Controllers/
│    └── LocationsController.cs
│
├── Data/
│    ├── ILocationRepository.cs
│    └── LocationRepository.cs
│
├── wwwroot/           # Static frontend files
│     ├── index.html
│     ├── app.js
│     └── style.css
│
├── Models
│      ├── DTO/
│      ├── Enum/
│      └── Location.cs
│
├── Properties
│      └── launchSettings.json
│
├── Services
│      ├── ILocationService.cs
│      ├── LocationService.cs
│      └── MapHub.cs
│
│
├── Program.cs
│
├── .gitignore
│
├── README.md
│
├── docker-compose.yml      #  MongoDB
│
└──  ecomapgbg.sln

````

---

  

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

## 📄 License


MIT License – feel free to reuse and contribute.

