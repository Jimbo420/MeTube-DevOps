# MeTube-DevOps

## Description
This is our group project for our DevOps course.

It's supposed to be a simple YouTube-inspired web application that demonstrates core video streaming concepts—uploading, searching, and playing videos—while focusing on clean design and user-friendly interactions.
The microservices that we managed to create are *Gateway*, *Client* and *UserService*.
But we only managed to finish the sign-up and log-in features during our short sprints.

Authors: Ali Behrooz, Ronnie Samson, Abdulla Mehdi, Oskar Andreasson, Sebastian Svensson - Loop Legion


## Table of Contents
- [Documentation](#documentation)
- [Application Architecture](#application-architecture)
- [Screenshots](#screenshots)
- [Sprints](#sprints)
  - [Sprint 1](#sprint-1)
  - [Sprint 2](#sprint-2)
  - [Sprint 3](#sprint-3)
  - [Sprint 4](#sprint-4)

## Application Architecture
![Application Architecture](./screenshots/apparch.png)

Diagram of our application architecture.

## Screenshots
Screenshot of the Signup page:

![Sign Up](./screenshots/signup.png)

This is where you intput your information to create an account.

---

Screenshot of the Login page:

![Login Page](./screenshots/login.png)

This is where you input you user information to login.

## Infrastructure as Code
Our infrastructure is managed using Terraform with Azure as the cloud provider.

**Prerequisites**
- Terraform CLI installed
- Azure CLI installed and authenticated
- Subscription ID for your Azure account

**Deployment Steps**
1. Set your Azure subscription ID:
```bash
$env:TF_VAR_subscription_id = "your-subscription-id"
```

2. Initialize Terraform:
```bash
cd terraform
terraform init
```

3. Deploy the infrastructure:
```bash
terraform apply
```

4. Retrieve the outputs:
```bash
terraform output
```
  Note the container registry hostname, username, and other outputs for use in deployment.

**Teardown**
To destroy the infrastructure:
```bash
terraform destroy
```

## CI/CD Pipeline
Our continuous integration and delivery pipeline is implemented using GitHub Actions.

### Pipeline Workflow

1. **Trigger**: The pipeline is triggered on:
- Push to master branch
- Pull request to master branch
- Manual dispatch via GitHub Actions UI

2. **Integration Phase**:
- Builds the application
- Runs unit tests
- Runs integration tests with SQL Server in Docker
- Lints the code

3. **Delivery Phase** (on successful merge to master):
- Builds Docker images
- Pushes images to Azure Container Registry
- Updates Kubernetes deployments

### Workflow Files
- userservice-ci.yml: CI pipeline for UserService
- .github/workflows/cd-workflow.yml: CD pipeline for deployment

## Application Programming Interface (API)

### UserService API

**User Management**

- **GET /api/User/manageUsers**
  - Returns a list of all users
  - Requires: Admin authentication
  - Response: Array of user objects
 
- **POST /api/User/signup**
  - Creates a new user account
  - Request Body:
```bash
{
  "username": "string",
  "password": "string",
  "email": "string",
  "role": "string"
}
```
  - Response: Created user object

- **POST /api/User/login**
  - Authenticates a user
  - Request Body:
```bash
{
  "username": "string",
  "password": "string"
}
```
  - Response: Authentication token






### Sprint 1
#### Planning
- **Sprint Backlog**:
  - *User Story 1*: As a user I should be able to sign up so that I have a user account.
  - *User Story 2*: As a developer, I want to setup Docker for the microservices.
  - *User Story 3*: As a developer I want the project to be setup with sub-projects for all microservices.

#### Review
- **Completed**:
  - *User Story 1*: Implementation of registration logic and functionality (Sign-Up).
  - *User Story 3*: Setup of microservice skeleton (not yet implemented).
- **Not Completed**:
  - *User Story 2*: Docker setup not implemented (Not enough time).
- **Issues**:
  - Integration with the database took longer than expected.
  - We had environment setup issues.
  - Very short sprint period, only 3 days.


#### Retrospective
- **What went well**:
  - Clear communication in daily stand-ups.
  - Smooth process for defining user stories.
  - Good distribution of tasks.
  - Strong attendance and engagement
#### Retrospective
- **What went well**:
  - Clear communication in daily stand-ups.
  - Smooth process for defining user stories.
  - Good distribution of tasks.
  - Strong attendance and engagement
- **What could be improved**:
  - Unclear distribution of roles in certain parts of API development.
- **Amendments to process**:
  - Reconsider role distribution and possibly adapt a more vertical working approach.
  - Formalize code review steps.

---
