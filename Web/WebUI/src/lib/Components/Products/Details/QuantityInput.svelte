<script lang="ts">
import { faCaretDown, faCaretUp } from "@fortawesome/free-solid-svg-icons";
import Fa from "svelte-fa";

    let quantity = 1;
</script>

<div class="container">
    <div class="items">
        <div class="buttons">
            <div class="button-up" on:click={() => {quantity+=1}}>
                <Fa icon={faCaretUp} size={'2x'} />
            </div>
            <div class="button-down" on:click={() => {
                quantity >= 1 ? quantity-=1 : null;
            }}>
                <Fa icon={faCaretDown} size={'2x'} color={quantity >= 1 ? '#000000' : 'gray'}/>
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
    }
    .button-down, .button-up {
        cursor: pointer;
    }
    .field {
        border: none;
        background-color:aliceblue;
        width: 8rem;
        height: 5rem;
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
