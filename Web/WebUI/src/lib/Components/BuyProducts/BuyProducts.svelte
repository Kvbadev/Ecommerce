<script lang="ts">
import PaymentForm from "./PaymentForm.svelte";
import {link, location, push} from 'svelte-spa-router'
import {userProfile} from '../../Stores/stores';
import { onDestroy } from "svelte";
import { oneTimeProduct } from "../../Stores/stores";


if($location == '/buyNow') {
    onDestroy(() => oneTimeProduct.set(null))
}
</script>
{#if !$userProfile?.username}
<h1>You must be <a href="/account/login" use:link>logged in </a>to buy from our store!</h1>
{:else}
<h1>Buy Products</h1>
<PaymentForm />
{/if}

<style>
    a {
        color: blue;
        text-decoration: underline;
    }
</style>