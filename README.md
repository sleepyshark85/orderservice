# Order Service

## About
Order service enables Art Directors and other client personnel to create, search, and check out orders, triggering automated workflows including payment processing, email notifications, invoicing, and production system integration. This repository contains all the system's architecture, design decisions, constraints, implementation details and tests for the service.

## Architecture

Our architecture documentation follows the [Arc42 template](https://arc42.org/overview) enhanced with C4 Model diagrams

1. **Introduction and Goals** - System purpose, quality goals, stakeholders
2. **Constraints** - Technical and business constraints
3. **Context and Scope** - Business context, technical context (featuring C4 Context diagram)
4. **Solution Strategy** - Overall approach and key decisions
5. **Building Block View** - System decomposition (featuring C4 Container and Component diagrams)
6. **Runtime View** - Key processes and workflows
7. **Deployment View** - Technical infrastructure and mapping
8. **Cross-cutting Concepts** - Overarching patterns and approaches
9. **Architecture Decisions** - Key decisions and their rationales
10. **Quality Requirements** - Quality scenarios and measures
11. **Risks and Technical Debt** - Known issues and future concerns
12. **Glossary** - Terms and definitions

Go to [docs/architecture.md](./docs/architecture.md) for more detailed information.

## Key Features

- Order search and filtering by name
- Checkout process workflow
- Production system integration
- Automated email notifications
- Invoice creation
- Payment processing
- Error handling and recovery


## How to run
Execute the following command at the root folder of the repository:


```
docker-compose up --build -d 
```

Applications url:

- Internal System: http://localhost:8080/scalar
- Checkout service: http://localhost:8082/scalar
- EventStore: http://localhost:2113/