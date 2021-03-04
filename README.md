# pact-net-01

## Getting Started

create a `.env` file in the root, use `.env.example` as guidance

## Contract Tests

based on:

- https://github.com/pact-foundation/pact-workshop-dotnet-core-v1
- https://github.com/pact-foundation/pact-net

### Pact Matchers

- .NET Matchers ([link](https://github.com/pact-foundation/pact-net/blob/eba0cb8d7e1172e92cfcdc22bb57bbb9ba188e13/Samples/EventApi/Consumer.Tests/EventsApiConsumerTests.cs#L261))

### Pact Broker

- CI/CD pipelines ([link](https://docs.pact.io/pact_nirvana/step_4))
- docs ([link](https://docs.pact.io/pact_broker))
  - webhooks ([link](https://docs.pact.io/pact_broker/webhooks))
  - pactflow webhooks ([link](https://docs.pactflow.io/docs/user-interface/settings/webhooks/))
  - tags ([link](https://docs.pact.io/pact_broker/tags))
  - setup checklist ([link](https://docs.pact.io/pact_broker/set_up_checklist/))
- wiki ([link](https://github.com/pact-foundation/pact_broker/wiki))
- dockerised pact broker ([link](https://hub.docker.com/r/pactfoundation/pact-broker))
- docker compose sample ([link](https://github.com/pact-foundation/pact-broker-docker/blob/master/docker-compose.yml))
- pact publish script (powershell) ([link](https://gist.github.com/neilcampbell/bc1fb7d409425894ece0))
- pact cli ([link](https://hub.docker.com/r/pactfoundation/pact-cli))
- publish docs ([link](https://github.com/pact-foundation/pact_broker-client#publish))
- can-i-deploy docs ([link](https://github.com/pact-foundation/pact_broker-client#can-i-deploy))
- codecentric blog
  - Consumer-driven contract testing with Pact ([link](https://blog.codecentric.de/en/2019/10/consumer-driven-contract-testing-with-pact/))
  - Message Pact – Contract testing in event-driven applications ([link](https://blog.codecentric.de/en/2019/11/message-pact-contract-testing-in-event-driven-applications/))
  - Implementing a consumer-driven contract testing workflow with Pact broker and GitLab CI ([link](https://blog.codecentric.de/en/2020/02/implementing-a-consumer-driven-contract-testing-workflow-with-pact-broker-and-gitlab-ci/))
- publish provider verification results to a broker ([link](https://github.com/pact-foundation/pact-net#publishing-provider-verification-results-to-a-broker))
- pactflow univeristy ([link](https://docs.pactflow.io/docs/workshops/))
- code samples ([link](https://docs.pactflow.io/docs/examples))
- videos
  - Pact tests: how we split up the monolithic deploy by Phil Hardwick ([link](https://www.youtube.com/watch?v=0sSy8ZTsW64))
  - Verifying Microservice Integrations with Contract Testing - Atlassian Summit 2016 ([link](https://www.youtube.com/watch?v=-6x6XBDf9sQ))
  - microXchg 2017 - Alon Pe'er: Move Fast and Consumer Driven Contract Test Things ([link](https://www.youtube.com/watch?v=nQ0UGY2-YYI))
  - YOW! 2017 Beth Skurrie - It's Not Hard to Test Smart: Delivering Customer Value Faster #YOW ([link](https://www.youtube.com/watch?v=79GKBYSqMIo))

#### Pact Broker Webhooks

Pact Content Changed

```shell
curl --location --request POST 'https://dev.azure.com/santi-azure-devops-01/_apis/public/distributedtask/webhooks/pact_content_changed?api-version=6.1-preview' \
--header 'Content-Type: application/json' \
--header 'Cookie: VstsSession=%7B%22PersistentSessionId%22%3A%227b562f9d-f74e-4021-9f02-4f0c34da9870%22%2C%22PendingAuthenticationSessionId%22%3A%2200000000-0000-0000-0000-000000000000%22%2C%22CurrentAuthenticationSessionId%22%3A%2200000000-0000-0000-0000-000000000000%22%2C%22SignInState%22%3A%7B%7D%7D; X-VSS-UseRequestRouting=True' \
--data-raw '{
    "consumerPactUri": "https://vasquezhouse.pactflow.io/pacts/provider/Something%20API/consumer/My%20Consumer%20NUnit/latest/refs%2Fheads%2Fmain"
}'
```

### Pact Wiki

- development workflow ([link](https://github.com/pact-foundation/pact-ruby/wiki/Development-workflow))
- using pact where the consumer team is different from the provider team ([link](https://github.com/pact-foundation/pact-ruby/wiki/Using-pact-where-the-consumer-team-is-different-from-the-provider-team))
- sharing pacts between consumer and provider ([link](https://github.com/pact-foundation/pact-ruby/wiki/Sharing-pacts-between-consumer-and-provider))
- other ([link](https://github.com/pact-foundation/pact-ruby/wiki))

## API Integration Tests

## Configuration via Environment Variables

Based on dotnet-env ([link 1](https://github.com/tonerdo/dotnet-env) | [link 2](https://mattcbaker.com/posts/using-dotenv-files-in-dotnet-core/))

## Terraforming

- azure, terraform, postgres ([link](https://techcommunity.microsoft.com/t5/azure-database-for-postgresql/using-terraform-to-create-private-endpoint-for-azure-database/ba-p/1276608))
- azure devops + terraform ([link 1](https://thomasthornton.cloud/2020/07/08/deploy-terraform-using-azure-devops/) | [link 2](https://thomasthornton.cloud/2020/09/22/deploying-terraform-from-develop-to-production-consecutively-using-azure-devops/) | [link 3](https://thomasthornton.cloud/2020/11/28/terraforming-from-zero-to-pipelines-as-code-with-azure-devops/))
- key vault ([link](https://jakewalsh.co.uk/automating-azure-key-vault-and-secrets-using-terraform/))
- nice sample ([link](https://medium.com/faun/terraform-series-01-scalable-app-service-using-azure-postgresql-db-part-1-9eb853bdad5c))

## Azure Pipelines

- Predefined variables ([link](https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml))
- Set variables in the pipeline ([link](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#setvariable-initialize-or-modify-the-value-of-a-variable))
- Service Containers ([link](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/service-containers?view=azure-devops&tabs=yaml))
- Trigger pipeline via webhooks ([link](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/resources?view=azure-devops&tabs=example#resources-webhooks))

# OpenAPI Specification

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-3.1
- https://ivanstambuk.github.io/azure/2020/10/16/Automatically-generation-OpenAPI-specification-in-ASP.NET.html
- https://github.com/domaindrivendev/Swashbuckle.AspNetCore