
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-RMCT5PJW.js",
      "chunk-3Y33IJ4N.js"
    ],
    "route": "/regions"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-QRPIEX6E.js",
      "chunk-E2MLBOG2.js"
    ],
    "route": "/provinces"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-QRPIEX6E.js",
      "chunk-E2MLBOG2.js"
    ],
    "route": "/provinces/*"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-BGICZTSR.js",
      "chunk-7UM346EW.js",
      "chunk-E2MLBOG2.js"
    ],
    "route": "/destinations"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-3ITXNXRG.js",
      "chunk-3Y33IJ4N.js",
      "chunk-E2MLBOG2.js"
    ],
    "route": "/culture"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-SNKXY3IE.js",
      "chunk-E2MLBOG2.js"
    ],
    "route": "/blog"
  },
  {
    "renderMode": 0,
    "redirectTo": "/",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 12415, hash: 'c5bda0c3ce4041596874b69471f47ff9913468b8399f526aa50830c52b108915', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1072, hash: '7a6015763d75fbe09cc6db66bcb6735ed78bcecb8d596e7caffeb25d9bfc7768', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'regions/index.html': {size: 31687, hash: 'bb8b03818ed865056ac4c38afb3f798ef274dac20c11a2da7c339671b0010d31', text: () => import('./assets-chunks/regions_index_html.mjs').then(m => m.default)},
    'index.html': {size: 88158, hash: '9a196e46bc6961521d12b2b0aecc4847dbfeb3e135be8204eb372b78cbb51daa', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'provinces/index.html': {size: 32164, hash: '2861b8a7a8ffcd9e0886aa41035c958e0be99a288e176d13abe4f8bad9204356', text: () => import('./assets-chunks/provinces_index_html.mjs').then(m => m.default)},
    'styles-DZCRIEDR.css': {size: 18374, hash: '1hferqk7rcc', text: () => import('./assets-chunks/styles-DZCRIEDR_css.mjs').then(m => m.default)}
  },
};
