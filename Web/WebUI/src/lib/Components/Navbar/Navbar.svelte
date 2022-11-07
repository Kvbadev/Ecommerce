<script lang="ts">
    import { faBars, faHamburger } from "@fortawesome/free-solid-svg-icons";
import Fa from "svelte-fa";
import NavItems from "./NavItems.svelte";
</script>
<!-- <div class="hamburger"> -->
    <!--TODO: implement hamburger-->
    <!-- <Fa icon={faBars} size={'4x'} /> -->
<!-- </div> -->
<nav>
    <ul class="nav-items">
        <NavItems />
    </ul>
</nav>
<div class="margin"></div>

<style>
    .hamburger {
        position: fixed;
        left: 95%;
        top: 1%;
    }
    nav {
        font-family: "Raleway", sans-serif;
        width: 100vw;
        height: 4.2vw;
        top: 0;
        position: fixed;
        z-index: 2;
        background: #d85818;
        background: -webkit-linear-gradient(top left, #d85818, #f9d14d);
        background: -moz-linear-gradient(top left, #d85818, #f9d14d);
        background: linear-gradient(to bottom right, #d85818, #f9d14d);
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
