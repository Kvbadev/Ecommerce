<script lang="ts">
import { agent } from "../../../Utils/agent";

import { onMount } from "svelte";
import { push } from "svelte-spa-router";

import { jwtToken, userProfile } from "../../../Stores/stores";
import Loader from "../../Common/Loader.svelte";
import { initShoppingCart } from "../../../Stores/ShoppingCartExtensions";

async function SignOut(){
    localStorage.removeItem("jwt");
    jwtToken.set('');
    userProfile.set(null);
    initShoppingCart(null);
    push('/');
}

onMount(() => {
    if(!localStorage.getItem("jwt")){
        push('/account/login');
    }
})
</script>


{#if !$userProfile}
    <Loader entire />
{:else}
<div class="container">
    <h1>{$userProfile.username}</h1>
    <p>{$userProfile.email}</p>
    <button class="signout" type="button" on:click={SignOut}>Sign out</button>
</div>
{/if}

<style>
.container *{
    margin: 2rem;
}
</style>