# 4. Solution strategy

To address the key requirements and quality goals, we've chosen the following strategic approaches:

- We use Sage pattern to orchestrate the critical payment process step, track the overall completion within out **Order Checkout System**
- We use Event-driven pattern to handle parallel communication with: Production System, Invoice System, Email Service ...

This approach gives us the benefit of both patterns:
- Central coordination and error handling through Saga.
- Independent, scalable services for paralel operations with Event-driven.