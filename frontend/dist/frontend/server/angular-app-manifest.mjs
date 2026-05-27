
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
      "chunk-P3G4IISZ.js"
    ],
    "route": "/regions"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-4AT5WWBR.js",
      "chunk-X5UXLVY5.js"
    ],
    "route": "/provinces"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-4AT5WWBR.js",
      "chunk-X5UXLVY5.js"
    ],
    "route": "/provinces/*"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-3VNFWCN5.js",
      "chunk-YJZX2XC3.js",
      "chunk-X5UXLVY5.js"
    ],
    "route": "/destinations"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-ZMBXYPM4.js",
      "chunk-X5UXLVY5.js"
    ],
    "route": "/culture"
  },
  {
    "renderMode": 0,
    "preload": [
      "chunk-QVPDPUB3.js",
      "chunk-X5UXLVY5.js"
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
    'index.csr.html': {size: 12466, hash: '9b6842e87133bc0aae4e5fc46c23225fbd15cf41096f80d4e8817f4814d19d46', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1123, hash: '4e6d42b33dbe2cf11d268b6e3c29b787961763ac93888ef3e8798d960c3f0466', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'regions/index.html': {size: 40345, hash: '5bee6c9c1cc9e01252194133cb0ce7830950198c708e78ca34efa46f095984a1', text: () => import('./assets-chunks/regions_index_html.mjs').then(m => m.default)},
    'index.html': {size: 88209, hash: '2828ec9d833dc9693fa508c8d1f63c263c2883f3531d102f2927e194a2235faf', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'provinces/index.html': {size: 39673, hash: 'e04bacb71a13ad31b859e64634fec9303d06755ff9b7a729bfc9fa4f50aa754c', text: () => import('./assets-chunks/provinces_index_html.mjs').then(m => m.default)},
    'styles-DZCRIEDR.css': {size: 18374, hash: '1hferqk7rcc', text: () => import('./assets-chunks/styles-DZCRIEDR_css.mjs').then(m => m.default)}
  },
};
