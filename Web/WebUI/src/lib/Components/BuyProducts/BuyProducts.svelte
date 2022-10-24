<script lang="ts">
import PaymentForm from "./PaymentForm.svelte";
import PurchaseSummarize from "./PurchaseSummarize.svelte";
import {link, location} from 'svelte-spa-router'
import {userProfile} from '../../Stores/stores';
import { onDestroy } from "svelte";
import { oneTimeProduct, products, shoppingCart } from "../../Stores/stores";
import Loader from "../Common/Loader.svelte";
import Fa from "svelte-fa";
import { faAngleDoubleLeft} from "@fortawesome/free-solid-svg-icons";


if($location === '/buyNow') {
    onDestroy(() => oneTimeProduct.set(null))
}
</script>

<div class="container">
{#if !$shoppingCart || !$products}
<Loader />
{:else}
    {#if !$userProfile?.username}
    <h1>You must be&nbsp;<a href="/account/login" use:link>logged in</a>&nbsp;to buy from our store!</h1>
    {:else}
    <h1>Buy Products</h1>
    <div class="checkout">
        <PaymentForm />
        <Fa icon={faAngleDoubleLeft} size={"5x"} />
        <PurchaseSummarize />
    </div>
    {/if}
{/if}
</div>

<style>
    a {
        color: blue;
        text-decoration: underline;
    }
    h1 {
        padding: 2rem 0;
        font-family: 'Raleway';
        font-size: 4rem;
        box-sizing: border-box;
        height: 15%;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .container {
        height: calc(100vh - 4.2vw);
        width: 100vw;
    }
    .checkout {
        display: flex;
        flex-direction: row;
        align-items: center;
        height: calc(85% - 2rem);
        box-sizing: border-box;
        margin-bottom: 2rem;
    }
</style>