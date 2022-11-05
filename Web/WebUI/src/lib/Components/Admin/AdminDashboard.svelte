<script lang="ts">
  import { faBox, faSignOut, faUser } from "@fortawesome/free-solid-svg-icons";
  import SideNav from "./SideNav.svelte";
  import {products} from '../../Stores/stores';
  import ProductManagement from './ProductManagement.svelte';
  import UsersManagement from "./UsersManagement.svelte";
  import { onMount } from "svelte";
  import { agent } from "../../Utils/agent";
  import { push } from "svelte-spa-router";
  import Loader from "../Common/Loader.svelte";



  let isPermitted = undefined;
  let page:0|1 = 0; //products 
  onMount(async () => {
    isPermitted = await agent.Account.isAdmin();
    if(!isPermitted) setTimeout(() => push('/'), 2000);
  })

</script>
{#if isPermitted === undefined || !$products}
<Loader />

{:else if isPermitted}

  <div class="container">
  <SideNav bind:page num={2} icons={[faBox, faUser, faSignOut]}/>

  {#if page === 0}
    <ProductManagement />
  {:else}
    <UsersManagement />
  {/if}
  </div>

{:else}
<h1>You are not permitted to use Administrator Dashboard</h1>
{/if}
  
<style>
  .container {
    display: flex;
    flex-direction: row;
    width: 100vw;
    height: 100vh;
  } 
</style>