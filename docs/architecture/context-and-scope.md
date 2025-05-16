# 3. Context and Scope

## 3.1. Business context

```mermaid
C4Context

Person(artDirectors, "Art Directors", "Searches for orders and initiates checkout process")

Enterprise_Boundary(enterprise, "Pixelz system") {
    System(orderCheckoutSystem, "Order Checkout System", "Order management system")

    System_Ext(paymentService, "Payment Service", "Processes payments")

    System_Boundary(postOrderProcessing, "Post-Order Processing") {
        System_Ext(productionSystem, "Production System", "Handles various concerns")
        System_Ext(emailService, "Email Service", "Sends notifications")
        System_Ext(invoiceSystem, "Invoice System", "Creates and manages invoices")
    }
}

Rel(artDirectors, orderCheckoutSystem, "Creates and checks out orders")
Rel_R(orderCheckoutSystem, emailService, "Sends notifications")
Rel_R(orderCheckoutSystem, invoiceSystem, "Creates invoices in")
Rel_D(orderCheckoutSystem, paymentService, "Processes payments")
Rel_D(orderCheckoutSystem, productionSystem, "Sends orders for production")
```

The System Context diagram illustrates the high-level interactions between the Order Checkout System and its environment:

- **Art Directors** interact with the system to search for orders and initiate checkout processes
- **Order Checkout System** allows **Art Directors** to search, manage and checkout orders
- External systems the **Order Checkout System** integrates with:
    - **Payment Service**: Processes payment transactions
    - **Email Service**: Sends confirmation emails to clients
    - **Production System**: Internal system that actually manage the orders and their status
    - **Invoice System**: Creates and manages invoices


## 3.2. Technical context

**Order Checkout System** interacts with the following external systems

|System             | Description           | Protocol              |
|-------------------|-----------------------|-----------------------|
| Payment Service   | Payment processing    | REST API              |
| Invoice System    | Invoices management   | Event subscription    |
| Production System | Orders management     | Event subscription    |
| Email Service     | Notification system   | Event subscription    |