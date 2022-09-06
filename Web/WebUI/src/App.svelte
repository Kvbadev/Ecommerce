<script lang="ts">
import Navbar from './lib/Components/Navbar/Navbar.svelte';
import { routes } from './routes';
import Router from 'svelte-spa-router'
import { onMount } from 'svelte';
import { agent } from './lib/Utils/agent';
import type Profile from './lib/Models/profile';
import type Cart from './lib/Models/cart';
import { jwtToken, products, userProfile } from './lib/Stores/stores';
import { initShoppingCart } from './lib/Stores/shoppingCartStore';

onMount(async () => {
  const jwt = localStorage.getItem("jwt");

  const localCart = JSON.parse(localStorage.getItem("cart")|| null) as Cart;
  initShoppingCart(localCart ?? null);

  if(jwt){
    const profile = await agent.Account.getProfile();
    if(profile) {
      jwtToken.set(jwt);
      userProfile.set(profile);
    } else { //token does not work anymore
      localStorage.removeItem("jwt");
    } 
  }
  // const prods = await agent.Products.getAll()
  products.set(await agent.Products.getAll());

})
</script>

  
<main>
  <Navbar />
</main>
<Router {routes} />
