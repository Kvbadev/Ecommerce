<script lang="ts">
  import { faBox, faSignOut, faUser } from "@fortawesome/free-solid-svg-icons";
  import SideNav from "./SideNav.svelte";
  import {products} from '../../Stores/stores';
  import ProductManagement from './ProductManagement.svelte';



  let page:0|1 = 0; //products 

</script>

<div class="container">
<SideNav bind:page num={2} icons={[faBox, faUser, faSignOut]}/>

{#if page === 0 && $products}
  <ProductManagement />
{/if}
  
</div>

<style>
  .container {
    display: flex;
    flex-direction: row;
    width: 100vw;
    height: 100vh;
  } 
</style>