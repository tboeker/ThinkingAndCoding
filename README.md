# ThinkingAndCoding
Think and Code

# Kubernetes

## Cluster Deployment

Ich verwende Azure Kubernetes Services (AKS). Der Cluster wird angelegt mittels der Azure Pipeline und Azure CLI. Das bestehende VNET sollte verwendet werden, damit ein Zugriff auf die MongoDb und EventStore Virtual Machines erfolgen kann.

## Install Frameworks

Wie werden die Komponenten wie Ingress, Admin Dashboard etc installiert? 
- Mit kubectl apply in der DevOps Pipeline
- Mit Keel? (https://keel.sh/)

## Install Own App

Wie wird unsere App später installiert?

# Health Checks

Readyness and Liveness Checks.... K8S schaltet erst aktiv wenn der Liveness erfüllt ist. 

Liveness prüft auf Self, ob der Pod am leben ist
Readyness prüft auf Services, ob der Pod korrekt hochgefahren und bereit ist

Readyness beinhaltet eigene Logik. So prüft zum Beispiel die Query API ob die Projection API online ist. Es werden ebenfalls Abhängigkeiten auf externe Dienste geprüft.

- https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

# App Startup / Lifetime

Wird eine eigene Lifetime benötigt? Kann man einfach IHostedService nutzen für Startup Init? Wir erfährt die API das die Lifetime aktiv ist? Geht der Liveness Check erst aktiv wenn alle Hosted Services durch sind?

(proj1)

- https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
- BackgroundService "blockiert" nicht den Startup
- IHostedService blockiert und wartet im Startup

Braucht man überhaupt eine API die schon endpunkte bereit stellt und es später erst verfügbar ist? Oder genügt es wenn bereit, dann bereit?

- Vielleicht kann man auch Background Services nutzen und dann mit Health Check prüfen ob alles da ist

# EventStore

Ich brauche einen Aggregate Store, ein Projection Setup, Projection.

Die Client Connection über HostedService erzeugen

# ReadModel Store

Werden wirklich Abstractions benötigt? Oder ist in einem Projekt immer der gleiche ReadModel Database Typ vorhanden? Zu Dev Zwecken wird sowieso per docker-compose alles gestartet was gebraucht wird

# Reporting

Der Stimulsoft JS sollte als Docker Image implementiert werden. Und dann per HTTP oder GRPC Call aufgerufen werden.

# MongoDb

Die Connection über einen Hosted Service

# CQRS

Man müsste die Query API und die Projector API koppeln. Der Query Service sollte erst healthy sein, wenn auch der Projector healthy ist

# Frameworking


# Versioning

GitVersion vs Minver 



# Build

Build in Docker vs Cake Build vs BullsEye and SimpleExec

Multi Project Docker Build aus der Solution

- https://github.com/adamralph/bullseye 
- https://www.softwaredeveloper.blog/multi-project-dotnet-core-solution-in-docker-image
- https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/

- Reference Assembly Versions, Props, Minver etc: https://github.com/IdentityServer/IdentityServer4