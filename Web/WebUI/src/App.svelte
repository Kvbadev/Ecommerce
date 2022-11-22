<script lang="ts">
import Navbar from './lib/Components/Navbar/Navbar.svelte';
import { routes } from './routes';
import Router from 'svelte-spa-router'
import { onMount } from 'svelte';
import { agent } from './lib/Utils/agent';
import { jwtToken, userProfile} from './lib/Stores/stores';
import { initShoppingCart } from './lib/Stores/ShoppingCartExtensions';
import type Cart from './lib/Models/cart';
import { SvelteToast, toast } from '@zerodevx/svelte-toast';
import {location} from 'svelte-spa-router';

onMount(async () => {

  const jwt = localStorage.getItem("jwt");
  
  if(jwt){
    let profile = await agent.Account.getProfile();

    if(profile) {
      jwtToken.set(jwt);
      userProfile.set(profile);

      const localCart: Cart = await agent.ShoppingCart.GetCart(); 
      initShoppingCart(localCart);

    } else { //token does not work anymore
      localStorage.removeItem("jwt");
      localStorage.removeItem("refresh");

      initShoppingCart(null);
      toast.push("You've been logged out");
    } 
  } else {
      initShoppingCart(JSON.parse(localStorage.getItem("cart"))||null);
  }
})
</script>

<!--if admin then not show-->
{#if $location.toLowerCase() !== '/admin'}
  <Navbar/>
{/if}
<Router {routes} />
<SvelteToast />
