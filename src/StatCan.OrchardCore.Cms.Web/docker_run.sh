#!/bin/bash
set -e

# Script used by the docker container to transform environment variables into orchard variables. $CONNSTRING_OPTIONS used to pass additional parameter to the connection strings
export OrchardCore__ConnectionString="User ID=$PGUSER;Password=$PGPASSWORD;Host=$PGHOST;Port=$PGPORT;Database=$PGDATABASE;$CONNSTRING_OPTIONS"
export OrchardCore__DatabaseProvider="Postgres"

dotnet StatCan.OrchardCore.Cms.Web.dll

exec "$@"