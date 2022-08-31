<script lang="ts">
import Navbar from './lib/Components/Navbar/Navbar.svelte';
import { routes } from './routes';
import Router from 'svelte-spa-router'
import { onMount } from 'svelte';
import { agent } from './lib/Utils/agent';
import { shoppingCart, updateShoppingCart, userProfile } from './lib/Stores/stores';
import type Profile from './lib/Models/profile';
import type {CartItem} from './lib/Models/cart';
import type Cart from './lib/Models/cart';

onMount(async () => {
  const jwt = localStorage.getItem("jwt");

  const localCart = JSON.parse(localStorage.getItem("cart")|| null) as Cart;

  if(localCart){
    shoppingCart.set(localCart);
  } else {
    shoppingCart.set({items: new Array<CartItem>, count: 0, sum: 0})
  }

  if(jwt){
    const profile:Profile = await agent.Account.getProfile() as Profile;
    userProfile.set(profile);
  }

})
</script>

  
<main>
  <Navbar />
</main>
<Router {routes} />
