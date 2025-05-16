# 1. Introduction and Goals

## Requirements Overview
The Order Checkout system enables Art Directors to search for previously created orders, initiate checkout processes, and push orders to the Production system. The primary workflow includes:

1. Art Directors searching and finding orders by name
2. Initiating checkout which processes payment
3. Upon successful payment:
    - Sending confirmation emails to clients
    - Creating invoices in the invoice system
    - Pushing orders to the internal Production system
4. Maintaining the order status throughout the process

## Quality Goals

|Priority | Quality Goal        | Motivation                                                                            |
|---------|---------------------|---------------------------------------------------------------------------------------|
| 1       | Reliability         | The checkout process must be reliable, handling partial failures without data loss    |
| 2       | Auditability        | All checkout actions must be fully auditable, creating a complete history             |
| 3       | Consistency         | Order state must remain consistent, even during system failures                       |
| 4       | Responsiveness      | API responses should be timely, with appropriate feedback during async processes      |
| 5       | Maintainability     | System should be easy to extend with new steps in the checkout process                |

## Stakeholders

|Role                               | Expectation                                                       |
|-----------------------------------|-------------------------------------------------------------------|
| Art Directors                     | Our end-users. We want to satisfy their needs                     |
| Owners of the external systems    | We use their services, we need to make sure we use them correctly |
| IT OPS                            | Operate and monitor our system                                    |