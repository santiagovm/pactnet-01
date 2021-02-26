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

- docs ([link](https://docs.pact.io/pact_broker))
  - webhooks ([link](https://docs.pact.io/pact_broker/webhooks))
  - setup checklist ([link](https://docs.pact.io/pact_broker/set_up_checklist/))
- dockerised pact broker ([link](https://hub.docker.com/r/pactfoundation/pact-broker))
- docker compose sample ([link](https://github.com/pact-foundation/pact-broker-docker/blob/master/docker-compose.yml))
- pact publish script (powershell) ([link](https://gist.github.com/neilcampbell/bc1fb7d409425894ece0))
- pact cli ([link](https://hub.docker.com/r/pactfoundation/pact-cli))
- can-i-deploy docs ([link](https://github.com/pact-foundation/pact_broker-client#can-i-deploy))

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