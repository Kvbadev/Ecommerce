<script lang="ts">
    import { faCaretDown, faCaretUp } from "@fortawesome/free-solid-svg-icons";
  import { modifyCart } from "../../../Stores/ShoppingCartExtensions";
    import Fa from "svelte-fa";
    export let quantity = 1;
    export let productID = null;
    export let ModifyCart = false;

    async function updateCart(val) {
        await modifyCart({id: productID, quantity: val}, true);
    }
</script>

<div class="container">
    <div class="items">
        <div class="buttons">
            <div class="button-up" on:click={() => {
                if(ModifyCart){
                    updateCart(1);
                } else {
                    quantity++;
                }
            }}>
                <Fa icon={faCaretUp} size={'2.5x'} />
            </div>
            <div class="button-down" on:click={() => {
                if(quantity > 1){
                    if(ModifyCart){
                        updateCart(-1);
                    } else {
                        quantity--;
                    }
                } 
            }}>
                <Fa icon={faCaretDown} size={'2.5x'} color={quantity > 1 ? '#000000' : '#D0D0D3'}/>
            </div>
        </div>
        <input class="field" bind:value={quantity} disabled/>
    </div>
</div>

<style>
    .items {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
        height: 100%
    }
    .button-down, .button-up {
        cursor: pointer;
    }
    .field {
        border: none;
        background-color:aliceblue;
        width: 10rem;
        height: 7rem;
        font-size: 4rem;
        text-align: center;
    } 
    .container {
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .buttons, .field{
        margin: 1rem;
    }
    .buttons {
        display: flex;
        flex-direction: column;
    }
</style>
