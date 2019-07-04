> Work in progress

# Rabbus
_RabbitMQ + Event Bus = Rabbus_

---

Rabbus is a simple event bus solution built on top of RabbitMQ and MediatR.

## Why should I to care?
Let's say we have various microservices (with isolated domain contexts) and we want to notify other microservices with a change that might also interest them.
In DDD, you should not update other domain contexts. Instead, you should publish domain events and let other domains handle their own logic.

## What's left?
This is just a quick solution I imported from a personal project of mine. There's still stuff left like:
- [ ] Resilience (Polly)
- [ ] Integration queries?
- [ ] Add support for MediatR notifications?

## Get started (ASP.NET Core)
1. Install the NuGet package (coming soon)
2. `TODO`
