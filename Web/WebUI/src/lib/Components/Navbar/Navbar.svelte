<script lang="ts">
    import NavItems from "./NavItems.svelte";
</script>

<nav>
    <ul class="nav-items">
        <NavItems />
    </ul>
</nav>
<div class="margin"></div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');
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
        align-items: center;
    }
    .margin {
        width: 100vw;
        height: 4.2vw;
    }
</style>
