<script lang="ts">
    import { faBars, faHamburger } from "@fortawesome/free-solid-svg-icons";
import Fa from "svelte-fa";
import NavItems from "./NavItems.svelte";
</script>

<nav>
    <ul class="nav-items">
        <NavItems />
    </ul>
</nav>
<div class="margin"></div>

<style>
    nav {
        font-family: "Raleway", sans-serif;
        width: 100vw;
        height: 4.2vw;
        top: 0;
        position: fixed;
        z-index: 2;
        backdrop-filter: blur(4rem);
        -webkit-backdrop-filter: blur(4rem);
        background-color: rgba(138, 138, 138, 0.6);
    }
    ul {
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: row;
        list-style-type: none;
        justify-content: space-between;
        align-items: center;
    }
    .margin {
        margin-bottom: 4.2vw;
    }
</style>
