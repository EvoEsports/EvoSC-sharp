<div align="center">
    <img src="./images/evosc_full.png" width="60%">
    <br>
    <img alt="GitHub" src="https://img.shields.io/github/license/EvoTM/EvoSC-sharp">
    <a href="https://sonarcloud.io/project/issues?resolved=false&types=CODE_SMELL&id=EvoTM_EvoSC-sharp"><img alt="Code smells" src="https://sonarcloud.io/api/project_badges/measure?project=EvoTM_EvoSC-sharp&metric=code_smells"></a>
    <a href="https://sonarcloud.io/project/issues?resolved=false&types=BUG&id=EvoTM_EvoSC-sharp"><img alt="Bugs" src="https://sonarcloud.io/api/project_badges/measure?project=EvoTM_EvoSC-sharp&metric=bugs"></a>
    <a href="https://sonarcloud.io/component_measures?metric=Coverage&id=EvoTM_EvoSC-sharp"><img alt="Code coverage" src="https://sonarcloud.io/api/project_badges/measure?project=EvoTM_EvoSC-sharp&metric=alert_status"></a>
    <a href="https://sonarcloud.io/component_measures?metric=Security&view=list&id=EvoTM_EvoSC-sharp"><img alt="Security rating" src="https://sonarcloud.io/api/project_badges/measure?project=EvoTM_EvoSC-sharp&metric=security_rating"></a>
    <a href="https://discord.gg/EvoTM"><img alt="Discord" src="https://img.shields.io/discord/384138149686935562?label=Discord&logo=discord&logoColor=fff"></a>
</div>

# EvoSC#

EvoSC# (spoken: EvoSC Sharp) is a server controller for Trackmania 2020 dedicated servers.

It has been written from the ground up to be modular, performant and easy to use.

It is currently still in development, so expect braking changes to happen at any time.

## Goals

The goal of this server controller is to replicate the functionality of the existing [EvoSC](https://github.com/evotm/EvoSC) and expand on it.

In general, we want to make it more user-friendly, more robust and generally also implement functionality that users have long wished for but we weren't able to implement in the older version due to Technical Debt.

For a roadmap of planned features and what we're currently working on, have a look at the [Project board](https://github.com/orgs/evoesports/projects/8).

## Support

* **WE WILL NOT BE RESPONSIBLE FOR ANY DAMAGE OR DATA LOST DUE TO USAGE OF THIS SOFTWARE.**
* **DO NOT USE IN A PRODUCTION SCENARIO, THE SOFTWARE IS STILL HEAVILY IN DEVELOPMENT.**
* **DO NOT ASK FOR ASSISTANCE IN USING THE SOFTWARE IN ITS UNFINISHED STATE.**

## Developing for EvoSC#

To setup a development environment for EvoSC#, we recommend having Docker installed and using the following Docker Compose template.
It sets up a TM2020 dedicated server for you as well as all the required other services.

```yml
version: "3.8"
services:
  trackmania:
    image: evotm/trackmania
    ports:
      - 2351:2350/udp
      - 2351:2350/tcp
      - "127.0.0.1:5001:5000/tcp" # Be careful opening XMLRPC to other hosts! Only if you really need to.
    environment:
      MASTER_LOGIN: "SERVERLOGIN" # Create server credentials at https://players.trackmania.com
      MASTER_PASSWORD: "SERVERPASS" # Create server credentials at https://players.trackmania.com
      XMLRPC_ALLOWREMOTE: "True"
    volumes:
      - UserData:/server/UserData
  db:
    image: mariadb
    restart: always
    ports:
      - "127.0.0.1:3306:3306"
    volumes:
      - MariaDBData:/var/lib/mysql
    environment:
      MARIADB_ROOT_PASSWORD: CHANGEME
      MARIADB_USER: evosc
      MARIADB_PASSWORD: evosc123!
      MARIADB_DATABASE: evosc
      MARIADB_AUTO_UPGRADE: always
  adminer:
    image: adminer
    restart: always
    ports:
      - "127.0.0.1:8081:8080"
volumes:
  UserData: null
  MariaDBData: null
```

We also have a documentation of the current code base available at https://evosc.io/.
