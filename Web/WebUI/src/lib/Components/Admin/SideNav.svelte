<script lang="ts">
  import { initShoppingCart } from "../../Stores/ShoppingCartExtensions";
  import { jwtToken, refreshToken, userProfile } from "../../Stores/stores";
  import Fa from "svelte-fa";
  import { push } from "svelte-spa-router";

    export let page; //page num
    export let num:number; //number of all the pages
    export let icons = [];

    const signOut = () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("refresh");
    jwtToken.set('');
    refreshToken.set('');
    userProfile.set(null);
    initShoppingCart(null);
    push('/');
    }
    
    function changePage(i) {
        if(i == icons.length-1) signOut();
        else page = i;
        return;
    }


</script>

<div class="container">
    <ul class="items">
        {#each Array(num+1) as _, i (i)}
            <li on:click={() => changePage(i)}>
                <Fa icon={icons[i]} size={'2.5x'} />
            </li>
        {/each}
    </ul>
</div>
<div class="barrier"></div>

<style>
    .barrier {
        width: 8vw;
        height: 100%;
    }
    .container {
        width: 8vw;
        height: 100vh;
        background-color: rgba(0, 0, 0, 0.1);        
        position: fixed;
    }
    ul {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    li {
        margin-top: 1.5rem;
        width: 80%;
        height: 6.5vw;
        display: grid;
        place-items: center;
        border-radius: 2rem;
        background-color: rgba(0, 0, 0, 0.1);
        transition: all 0.5s ease-in-out;
    }
    li:hover {
        background-color: rgba(255, 255, 255, 0.8);
        cursor: pointer;
    }
    li:last-child {
        margin-top: auto;
        margin-bottom: 1.5rem;
    }
</style>