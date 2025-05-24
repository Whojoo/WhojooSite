CREATE SCHEMA IF NOT EXISTS fuel;

CREATE TABLE IF NOT EXISTS fuel.object_type
(
    id
    uuid
    PRIMARY
    KEY,
    type_name
    varchar
(
    20
) NOT NULL,
    creation_date timestamptz NOT NULL
    );

CREATE TABLE IF NOT EXISTS fuel.trackable_object
(
    id
    uuid
    PRIMARY
    KEY,
    identifier
    varchar
(
    50
) NOT NULL,
    object_type uuid references fuel.object_type
(
    id
),
    owner_id uuid NOT NULL,
    creation_date timestamptz NOT NULL,
    UNIQUE
(
    id,
    identifier
)
    );

CREATE TABLE IF NOT EXISTS fuel.fuel_entry
(
    id
    uuid
    PRIMARY
    KEY,
    mileage
    integer,
    creation_date
    timestamptz
    NOT
    NULL,
    execution_date
    timestamptz,
    refuel_amount
    float8
    NOT
    NULL,
    fuel_liter_price
    money,
    fuel_total_price
    money,
    object_id
    uuid
    references
    fuel
    .
    trackable_object
(
    id
)
    );

INSERT INTO fuel.object_type (id, type_name, creation_date)
VALUES ('0197022b-e5ed-779f-aa77-7646a3dd781b', 'car', CURRENT_TIMESTAMP),
       ('0197022f-0c9c-741e-9c6c-7e2c767b9c01', 'motorcycle', CURRENT_TIMESTAMP) ON CONFLICT
ON CONSTRAINT object_type_pkey DO NOTHING;
