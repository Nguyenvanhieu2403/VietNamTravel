import './polyfills.server.mjs';
import{fb as t,i as r,l as o}from"./chunk-GRQFWDQV.mjs";var n=class e{constructor(i){this.apiService=i}getRegions(){return this.apiService.get("regions")}getRegionBySlug(i){return this.apiService.get(`regions/${i}`)}static \u0275fac=function(a){return new(a||e)(o(t))};static \u0275prov=r({token:e,factory:e.\u0275fac,providedIn:"root"})};export{n as a};
