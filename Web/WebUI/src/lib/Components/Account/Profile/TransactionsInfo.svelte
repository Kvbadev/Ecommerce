<script lang="ts">
import { agent } from "../../../Utils/agent";
import Loader from "../../Common/Loader.svelte";
import CartProduct from "../../ShoppingCart/CartProduct.svelte";
import { link, push } from "svelte-spa-router";
import Fa from "svelte-fa";
import { faQuestionCircle } from "@fortawesome/free-solid-svg-icons";
  import { onMount } from "svelte";

const onQuestion = (id: string) => push(`/help/${id}`);
onMount(async() => {
})
</script>

<div class="container">
    
    <h1>Previous Purchases</h1>
    {#await agent.Account.getTransactions()}
        <Loader inElement color={'#000000'} size={5}/>
    {:then value} 
    {#if !value.length}
        <h3>We are waiting for Your first order :</h3>
    {:else}
        <ul class="list">
        {#each value as t}
        <li class="element">
            <div class="div-id">
                <h2>ID</h2>
                <a href={`/transaction/${t.id}`} use:link class="transaction-id">{t.id}</a>
            </div>
            <ul>
            {#if t.products.length}
                {#each t.products as prod}
                    <li class="item">
                        <CartProduct prod={prod} simplified size={40}/>
                    </li>
                {/each}
                {:else}
                    <li class="item">
                        <h4>Products have been removed 
                            <span on:click={()=>onQuestion(t.id)}><Fa icon={faQuestionCircle}/></span>
                        </h4>
                    </li>
            {/if}
            </ul>
            <div class="div-info">
                <h2>Price: {t.price}$</h2>
                <h2>Date: {t.issuedAt.toString().split('T')[0]}</h2>
            </div>
        </li>
        {/each}
        </ul>   
    {/if}
    {/await}
</div>

<style>
    h3 {
        width: 100%;
        height: 80%;
        display: grid;
        font-size: 4rem;
        font-family: 'Anek Telugu' !important;
        place-content: center;
    }
    
    h4 {
        width: 100%;
        font-size: 2rem;
    }
    h4 span {
        cursor: pointer;
    }
    .container {
        width: 100%;
        position: relative;
        background-color: rgb(239, 248, 255);
        overflow: auto;
        margin-bottom: 1rem;
    }
    ul {
        list-style: none !important;
    }
    .item {
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .div-info {
        background-color: rgb(241, 241, 241);
        border-radius: 1rem;
    }
    .div-info * {
        padding: 0;
        margin: 0 1rem;
        display: block;
        text-align: center;
        font-size: 2.5rem !important;
    }
    .element {
        border: solid black 0.2rem;
        border-radius: 1rem;
        margin: 1rem 0;
        max-height: 100%;
        display: flex;
        flex-direction: column;
        background-color: white;
        justify-content: space-around;
        width: 95%;
    }

    .element > ul {
        border-radius: 3rem;
        margin: 1rem;
        display: flex;
        height: 26rem;
        flex-direction: column;
        border-radius: 1rem;
        overflow: scroll;

    }

    .div-id {
        background-color: rgb(241, 241, 241);
        display: flex;
        flex-direction: row;
        justify-content: center;
        align-items: center;
        border-radius: 1rem;
    }

    .list {
        font-family: 'Raleway';
        display: grid;
        place-items: center;
        height: 80%;
        
        /* grid-template-columns: 1fr 1fr; */
        grid-template-columns: repeat(auto-fit, minmax(40%, 1fr))
        /* grid-template-rows: minmax(auto, 27rem) 27rem; */
    }
    h2 {
        display: inline-block;
        padding: 1rem;
        font-size: 3rem;
        color: gray;
    }
    .transaction-id {
        color: blue;
        text-align: center;
        font-family: 'Gemunu Libre';
        font-size: 2rem;
    }
    h1 {
        font-family: 'Raleway';
        font-size: 5rem;
        background-color: rgb(201, 216, 228);
        height: 20.3%;
        display: grid;
        place-content: center;
    }
</style>