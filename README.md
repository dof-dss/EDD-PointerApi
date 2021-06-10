# EDD-PointerApi

## Description
This is a simple unambitious postcode address lookup Api for Northern Ireland addresses.

e.g. from a consuming application the user adds a postcode

![Uploading lookup.png…]()

## Contents of this file

- [Contributing](#contributing)
- [Licensing](#licensing)
- [Project Documentation](#project-documentation)
    - [Why did we build this project](#why-did-we-build-this-project)
    - [What problem was it solving](#what-problem-was-it-solving)
    - [How did we do it](#how-did-we-do-it)
    - [Future Plans](#future-plans)
    - [Deployment Guide](#deployment-guide)

## Contributing

Contributions are welcomed! Read the [Contributing Guide](./docs/contributing/Index.md) for more information.

## Licensing

Unless stated otherwise, the codebase is released under the MIT License. This covers both the codebase and any sample code in the documentation. The documentation is © Crown copyright and available under the terms of the Open Government 3.0 licence.

## Project Documentation

### Why did we build this project?

We built this so applications can allow users to enter their postcode and recieve a list of addresses, in order to select their address. We built this api so the data and functionality can be shared by many applications.

### What problem was it solving?

This solves having to create a pointer table in every single application and adding the same code over and over again. There are 3 main endpoints:

- Search by postcode
- Search by postcode and premises number
- Search by x and y co-ordinates which is handy for plotting a point on a map for example

### How did we do it?

This is a dotnet core application which uses Mysql to store the pointer data, Entity Framework for data access and JWT to authenticate applications to allow them to use the api.

### Future plans

We may introduce a more advanced search if needed.

### Deployment guide

To run the databases you need mysql installed. Then run the below commands to set up the database:

- update-database

Restore the nuget package. Then to build run "dotnet build" in command line then dotnet run to run the site.

### Dataset

You can obtain the dataset which is around one million addresses from OSNI / LPS in csv format and manually input this into the database.



