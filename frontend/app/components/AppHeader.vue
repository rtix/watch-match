<template>
  <header class="header">
    <nuxt-link to="/">
      <span class="header__title">Watch Match</span>
    </nuxt-link>
  </header>
</template>

<script lang="ts" setup></script>

<style>
.header {
  position: relative;
  display: flex;
  justify-content: center;

  padding-block: 0.5rem;
  background: linear-gradient(to top, rgba(0, 0, 0, 0.2), #00000000);
}

.header__title {
  font-size: 1.5rem;
  font-weight: bold;
}
</style>
