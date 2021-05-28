ROLLBACK;
BEGIN;

DROP TABLE public.images_tags;
DROP TABLE public.images_albums;
DROP TABLE public.image_details;
DROP TABLE public.images;
DROP TABLE public.albums CASCADE;
DROP TABLE public.privacy;
DROP TABLE public.tags;

CREATE TABLE application_users();

CREATE TABLE public.privacy
(
    privacy_id serial NOT NULL,
    name character varying(20) NOT NULL,
    PRIMARY KEY (privacy_id)
)
WITH (
    OIDS = FALSE
);

CREATE TABLE public.albums
(
    album_id serial,
    user_id character varying(40),
    privacy_id integer,
    name character varying,
    description text,
    PRIMARY KEY (album_id),
	/*FOREIGN KEY (user_id) REFERENCES public.users (user_id) ON UPDATE CASCADE ON DELETE CASCADE,*/
	FOREIGN KEY (privacy_id) REFERENCES public.privacy (privacy_id) ON UPDATE CASCADE
)
WITH (
    OIDS = FALSE
);

CREATE TABLE public.images
(
    image_id serial NOT NULL,
    user_id character varying(40),
    album_id integer,
    image_sha1 character varying(40),
    PRIMARY KEY (image_id),
	--FOREIGN KEY (user_id) REFERENCES public.users (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (album_id) REFERENCES public.albums (album_id) ON UPDATE CASCADE
)
WITH (
    OIDS = FALSE
);

CREATE TABLE public.image_details
(
    image_id integer NOT NULL,
    name character varying(40),
    description text,
    date date,
    width integer,
    height integer,
    original_name character varying,
    PRIMARY KEY (image_id),
	FOREIGN KEY (image_id) REFERENCES public.images (image_id) ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
);

CREATE TABLE public.tags
(
    tag_id serial NOT NULL,
    name character varying(40),
    PRIMARY KEY (tag_id)
)
WITH (
    OIDS = FALSE
);

CREATE TABLE public.images_tags
(
    tag_id integer,
    image_id integer,
	FOREIGN KEY (tag_id) REFERENCES public.tags (tag_id) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (image_id) REFERENCES public.images (image_id) ON UPDATE CASCADE
)
WITH (
    OIDS = FALSE
);

/* many images in many albums */
CREATE TABLE public.images_albums
(
    images_album_id integer,
    albums_album_id integer
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.images_albums
    ADD FOREIGN KEY (albums_album_id)
    REFERENCES public.albums (album_id)
    NOT VALID;

ALTER TABLE public.images_albums
    ADD FOREIGN KEY (images_album_id)
    REFERENCES public.images (image_id)
    NOT VALID; 

END;