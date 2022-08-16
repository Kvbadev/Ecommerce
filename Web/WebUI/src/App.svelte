<script lang="ts">
import Navbar from './lib/Components/Navbar/Navbar.svelte';
import { routes } from './routes';
import Router from 'svelte-spa-router'
</script>
  
<main>
  <Navbar />
</main>
<Router {routes} />
