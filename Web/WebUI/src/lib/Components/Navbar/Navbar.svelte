<script lang="ts">
    import Logo from "./Logo.svelte";
    import NavItems from "./NavItems.svelte";
</script>

<nav>
    <ul class="nav-items">
        <NavItems />
    </ul>
</nav>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Raleway&display=swap');
    nav {
        font-family: "Raleway", sans-serif;
        width: 100vw;
        height: 10vh;
        position: sticky;
        left: 0;
        top: 0;
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
</style>
