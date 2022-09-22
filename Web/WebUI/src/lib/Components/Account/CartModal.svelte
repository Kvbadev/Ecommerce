<script lang="ts">
import { faWindowClose } from "@fortawesome/free-solid-svg-icons";
import { initShoppingCart, saveLocalCart} from "../../Stores/ShoppingCartExtensions";
import { agent } from "../../../lib/Utils/agent";
import Fa from "svelte-fa";
import { push } from "svelte-spa-router";


export let message = 'text';
export let buttons: Array<{text: string, type: "good"|"bad"|"neutral"}> = [{text: "text", type: "good"}];
export let showModal = false;

function hideModal () {
    showModal = false;
}
async function setNewCart() {
    saveLocalCart();
}
async function onButton() {
    hideModal();
    initShoppingCart(await agent.ShoppingCart.GetCart());
    push('/');
}

</script>

<div class="container" on:click={hideModal}>
    <div class="close" on:click={hideModal}><Fa icon={faWindowClose} size="lg" /></div>
    <div class="items" on:click={(e) => e.stopPropagation()}>
        <h1>{message}</h1>
        <div class="buttons">
        {#each buttons as btn}
            <button class={btn.type} on:click={async () => {
                if(btn.text.includes('Persist')){
                    await setNewCart();
                }
                onButton();
        }} type="button">{btn.text}</button>
        {/each}
        </div>
    </div>
</div>

<style>
    .good {
        background-color: green;
    }
    .bad {
        background-color: red;
    }
    .close {
        cursor: pointer;
        position: relative;
        bottom: 17.2%;
        left: 36.5%;
        z-index: 1;
        width: 3rem;
        text-align: center;
    }
    .container {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        backdrop-filter: brightness(40%);
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .items {
        backdrop-filter: brightness(100%);
        display: flex;
        width: 60rem;
        height: 25rem;
        background-color: white;
        flex-direction: column;
        justify-content: space-evenly;
        align-items: center;
    }
    button {
        cursor: pointer;
        width: 22rem;
        display: block;
        margin: 1rem;
        height: 3rem;
    }
    h1 {
        font-size: 3rem;
    }

</style>