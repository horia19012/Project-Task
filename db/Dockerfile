FROM mcr.microsoft.com/mssql-tools

WORKDIR /db

COPY db.sql ./db.sql
COPY db_populate.sql ./db_populate.sql
COPY init-db.sh ./init-db.sh

RUN chmod +x ./init-db.sh

ENTRYPOINT ["/bin/bash", "/db/init-db.sh"]