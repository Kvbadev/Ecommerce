<script lang="ts">
import Navbar from './lib/Components/Navbar/Navbar.svelte';
import { routes } from './routes';
import Router from 'svelte-spa-router'
import { onMount } from 'svelte';
import { agent } from './lib/Utils/agent';
import type User from './lib/Models/user';
import { userProfile } from './lib/Stores/stores';
import type Profile from './lib/Models/profile';

onMount(async () => {
  if(localStorage.getItem("jwt")){
    const profile:Profile = await agent.Account.getProfile();
    userProfile.set(profile);
  }
})
</script>

  
<main>
  <Navbar />
</main>
<Router {routes} />
