<script lang="ts">
  import { faClose } from "@fortawesome/free-solid-svg-icons";
  import { products } from "../../Stores/stores";
import Fa from "svelte-fa";
  import { fade } from "svelte/transition";
  import Loader from "../Common/Loader.svelte";
  import { agent } from "../../Utils/agent";
  import { onMount } from "svelte";
  import { get } from "svelte/store";
  import type Product from "../../Models/product";

    export let edit = "";

    let prod = {} as Product|any, dirty=false;
    let submitting = false;
    const up = (str: string) => str.charAt(0).toUpperCase() + str.slice(1);

    const save = async () => {
        submitting = true;
        const tmp = {};
        try{
            prod.photos = typeof prod.photos === typeof "" ?
                prod.photos.replace(/ /g,'').split(',') : prod.photos;

            for(const pr in $products[0]){
                if(pr !== 'id')
                    tmp[pr] = prod[pr];
            }
            if(edit === 'new')
            {
                const newProd = await agent.Products.create(tmp); 
                $products = [...$products, newProd] //refresh store
            } 
            else
            {
                await agent.Products.edit(edit, tmp);
                $products = [...$products] //refresh store
            }
            submitting = false;
            edit = "";
        } catch(e)
        {
            console.error(e);
            submitting = false;
            dirty = false;
        }

    }

    onMount(async () => {
        if(edit !== 'new')
            prod = get(products).find(x => x.id === edit);
    })
</script>

<div class="container" in:fade on:click={() => edit=""}>
    <span on:click={() => edit=""}><Fa icon={faClose} color="black" size="3x"/></span>
    <div class="items" on:click={e => e.stopImmediatePropagation()}>
        <h1>{edit === 'new' ? 'New Product' : $products.find(x => x.id === edit)?.name}</h1>
        <form on:submit|preventDefault={save}>
            {#each Object.keys($products[0]).filter(x => x!=='id') as key}
                <div class={key}>
                    {up(key)}: 
                    {#if key==='price'}
                    <input type="number" on:input|once={() => dirty=true} class="key" bind:value={prod[key]} >
                    {:else if key==='Photos'}
                    <input on:input|once={() => dirty=true} class="key" 
                    bind:value={prod[key]}
                    >
                    {:else}
                    <input class="key" on:input|once={() => dirty=true} bind:value={prod[key]} >
                    {/if}
                </div>
            {/each}
        </form>
        <div class="buttons">
        <button class='cancel' type="button" on:click={() => edit = ""}>Cancel</button>
        <button class={dirty ? 'save' : 'disable'} type="submit" on:click={save} disabled={!dirty}>
            {#if submitting}
            <Loader inElement size={1} color={'#ffffff'}/>
            {:else}
            Save
            {/if}
        </button>
        </div>
    </div>
</div>

<style>
    .container {
        position: fixed;
        width: 100vw;
        z-index: 100;
        height: 100vh;
        display: grid;
        place-items: center;
        backdrop-filter: brightness(30%);
        cursor: pointer;
    }
    .buttons {
        width: 80%;
        height: 15%;
        display: flex;
        flex-direction: row;
        align-items: flex-start;
        justify-content: center;
    }
    .buttons button {
        position: relative;
        cursor: pointer;
        border-top-right-radius: 0.7rem;
        border-bottom-left-radius: 0.7rem;
        margin: 0 0.5rem;
        border: none;
        color: white;
        background-color: red;
        transition: 0.3s ease-in;
        width: 35%;
        height: 50%;
    }
    .save {
        position: relative;
        background-color: green!important;
    }
    .disable {

        background-color: gray!important;
    }
    input {
        text-align: center;
        transition: all 0.3s;
    }
    input:focus {
        border-left: 0.4rem blue solid;
        /* border: none; */
        outline: none;
    }

    .description {
        overflow: scroll;
    }
    .container > div {
        background-color: rgba(255, 255, 255, 0.9);        
        width: 60%;
        height: 80%;
        display: flex;
        align-items: center;
        justify-content: space-evenly;
        flex-direction: column;
    }

    h1 {
        font-size: 4rem;
        height: 20%;
        font-family: 'Rubik';
        display: flex;
        align-items: center;
    }

    form {
        width: 60%;
        height: 80%;
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
    }
     
    form > div {
        font-size: 2.5rem;;
        font-family: 'Raleway';
        width: 80%;
        display: flex;
        margin: 1rem;
        justify-content: space-between;

    }

    input {
        padding: 0 0.5rem;
        width: 70%;
        height: 4rem;
        border: 0.1rem solid black;
    }

    span {
        position: absolute;
        top: 10%;
        left: 77.5%;
        z-index: 2;
        cursor: pointer;
    }
   
</style>