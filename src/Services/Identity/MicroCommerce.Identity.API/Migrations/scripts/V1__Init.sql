CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

CREATE TABLE "AspNetRoles" (
    id text NOT NULL,
    name character varying(256) NULL,
    normalized_name character varying(256) NULL,
    concurrency_stamp text NULL,
    CONSTRAINT pk_roles PRIMARY KEY (id)
);

CREATE TABLE "AspNetUsers" (
    id text NOT NULL,
    user_name character varying(256) NULL,
    normalized_user_name character varying(256) NULL,
    email character varying(256) NULL,
    normalized_email character varying(256) NULL,
    email_confirmed boolean NOT NULL,
    password_hash text NULL,
    security_stamp text NULL,
    concurrency_stamp text NULL,
    phone_number text NULL,
    phone_number_confirmed boolean NOT NULL,
    two_factor_enabled boolean NOT NULL,
    lockout_end timestamp with time zone NULL,
    lockout_enabled boolean NOT NULL,
    access_failed_count integer NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (id)
);

CREATE TABLE "AspNetRoleClaims" (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    role_id text NOT NULL,
    claim_type text NULL,
    claim_value text NULL,
    CONSTRAINT pk_role_claims PRIMARY KEY (id),
    CONSTRAINT fk_role_claims_asp_net_roles_role_id FOREIGN KEY (role_id) REFERENCES "AspNetRoles" (id) ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    id integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    user_id text NOT NULL,
    claim_type text NULL,
    claim_value text NULL,
    CONSTRAINT pk_user_claims PRIMARY KEY (id),
    CONSTRAINT fk_user_claims_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    login_provider character varying(128) NOT NULL,
    provider_key character varying(128) NOT NULL,
    provider_display_name text NULL,
    user_id text NOT NULL,
    CONSTRAINT pk_user_logins PRIMARY KEY (login_provider, provider_key),
    CONSTRAINT fk_user_logins_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    user_id text NOT NULL,
    role_id text NOT NULL,
    CONSTRAINT pk_user_roles PRIMARY KEY (user_id, role_id),
    CONSTRAINT fk_user_roles_roles_role_id FOREIGN KEY (role_id) REFERENCES "AspNetRoles" (id) ON DELETE CASCADE,
    CONSTRAINT fk_user_roles_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
);

CREATE TABLE "AspNetUserTokens" (
    user_id text NOT NULL,
    login_provider character varying(128) NOT NULL,
    name character varying(128) NOT NULL,
    value text NULL,
    CONSTRAINT pk_user_tokens PRIMARY KEY (user_id, login_provider, name),
    CONSTRAINT fk_user_tokens_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
);

CREATE INDEX ix_role_claims_role_id ON "AspNetRoleClaims" (role_id);

CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" (normalized_name);

CREATE INDEX ix_user_claims_user_id ON "AspNetUserClaims" (user_id);

CREATE INDEX ix_user_logins_user_id ON "AspNetUserLogins" (user_id);

CREATE INDEX ix_user_roles_role_id ON "AspNetUserRoles" (role_id);

CREATE INDEX "EmailIndex" ON "AspNetUsers" (normalized_email);

CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" (normalized_user_name);

INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
VALUES ('20201215170813_Init', '5.0.1');

COMMIT;
