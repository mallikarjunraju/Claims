# Claims API Documentation

## Overview

This API manages insurance claims and covers, allowing operations such as creation, retrieval, and deletion of claim and cover entries.

## Endpoints

### Claims

- POST /claims: Create a claim.
- GET /claims: List all claims.
- GET /claims/{id}: Retrieve a specific claim.
- DELETE /claims/{id}: Delete a specific claim.

### Covers

- POST /covers: Create a cover.
- GET /covers: List all covers.
- POST /covers/compute: Calculate cover details.
- GET /covers/{id}: Retrieve a specific cover.
- DELETE /covers/{id}: Delete a specific cover.




## Task 1

1. Code is segregated into their respective projects (Followed Onion Architecture). 
1. Used `MediatR` pattern for loose coupling between Api and Application layer.
1. Used `SOLID` principles.
1. Documentation is added.

## Task 2

1. Fluent validation rules are added for the request validation.
1. Custom exception handling is added too for specific exceptions.

## Task 3

1. I am not entirely clear with this task. Generally, auditing is done once the transaction (INSERT & DELETE) is completed but the task is contrary to this. I would keep the auditing out of the claims api. (Open for discussion.)
1. However, I have used Task.WhenAll to make the process async and non-blocking.

## Task 4

1. I have added a few unit tests for the Claims API. There is definitely room for more tests but due to time constraints, I have important ones. (Only postive tests.)

## Task 5

1. Refactored the code and corrected the logic as per the requirement.
