<?php
/**
 * The base configuration for WordPress
 *
 * The wp-config.php creation script uses this file during the
 * installation. You don't have to use the web site, you can
 * copy this file to "wp-config.php" and fill in the values.
 *
 * This file contains the following configurations:
 *
 * * MySQL settings
 * * Secret keys
 * * Database table prefix
 * * ABSPATH
 *
 * @link https://codex.wordpress.org/Editing_wp-config.php
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define( 'DB_NAME', 'cowrysol_wp815' );

/** MySQL database username */
define( 'DB_USER', 'cowrysol_outdare' );

/** MySQL database password */
define( 'DB_PASSWORD', 'cowrys0l!!');

/** MySQL hostname */
define( 'DB_HOST', 'localhost' );

/** Database Charset to use in creating database tables. */
define( 'DB_CHARSET', 'utf8mb4' );

/** The Database Collate type. Don't change this if in doubt. */
define( 'DB_COLLATE', '' );

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define( 'AUTH_KEY',         '8cqot8z69wgmnfq1ilcnkkdwkxlh0efvn8x4xpxawahmwyhds2vwjrvicskxdm2r' );
define( 'SECURE_AUTH_KEY',  'hshmltvnlhnt0wch1t9yxjynh9ck8sjj84favzbassfujesilqtafmine3dpdmzu' );
define( 'LOGGED_IN_KEY',    'fhwvh2vsa9cmkaellakzhk6rfvyalwypaad6lmz2ijbwqr0jllgob2mldnaqehhf' );
define( 'NONCE_KEY',        'nqgklnq6iryivbkvadj7nztmlnvazfr5py7qwthhrsmhi8iynvlh7rouzzacyy0t' );
define( 'AUTH_SALT',        '2ow5um3nxhbk24f6togxquycvougtojofk6aeqhruot2epdt5i5ejkiaxokfguzl' );
define( 'SECURE_AUTH_SALT', 'bffonmyfpxbbpdpqozyautzfvdcforq4nfphalmefh1isliqc3nmayc5zea1bpoq' );
define( 'LOGGED_IN_SALT',   'icabolwgddk6bsr4ovigbhomtmrx3vetutjobyoqfc4dwqfcjyoai8uc9dtwxobv' );
define( 'NONCE_SALT',       'adnimoy7myd8409grlfmoiedgus2zj6siztrbyuvxn5iwmxtpkn4c1ozbwqtk4gn' );

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each
 * a unique prefix. Only numbers, letters, and underscores please!
 */
$table_prefix = 'wpfi_';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 *
 * For information on other constants that can be used for debugging,
 * visit the Codex.
 *
 * @link https://codex.wordpress.org/Debugging_in_WordPress
 */
define( 'WP_DEBUG', false );

/* That's all, stop editing! Happy publishing. */

/** Absolute path to the WordPress directory. */
if ( ! defined( 'ABSPATH' ) ) {
	define( 'ABSPATH', dirname( __FILE__ ) . '/' );
}

/** Sets up WordPress vars and included files. */
require_once( ABSPATH . 'wp-settings.php' );
