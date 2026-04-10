#!/bin/bash
set -euo pipefail

if [ -z "${MSSQL_SA_PASSWORD:-}" ]; then
  echo "MSSQL_SA_PASSWORD is not set."
  exit 1
fi

SQLCMD_BIN="/opt/mssql-tools/bin/sqlcmd"
SQLCMD="${SQLCMD_BIN} -C -S sqlserver,1433 -U sa -P ${MSSQL_SA_PASSWORD}"

echo "Waiting for SQL Server to accept connections..."
until ${SQLCMD_BIN} -C -S sqlserver,1433 -U sa -P "${MSSQL_SA_PASSWORD}" -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 2
done

echo "Running schema script..."
$SQLCMD -i /db/db.sql

echo "Running seed script..."
$SQLCMD -i /db/db_populate.sql

echo "Database initialization completed."
touch /tmp/db-init.done

# Keep the container alive so Compose can treat it as a healthy dependency.
tail -f /dev/null