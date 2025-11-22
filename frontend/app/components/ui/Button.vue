<template>
  <button class="button" :checked.attr="checked">
    <slot/>
  </button>
</template>

<script lang="ts" setup>
defineProps<{ checked?: boolean }>();
</script>

<style scoped>
.button {
  background: none;
  border: none;
  font-size: inherit;
  display: flex;
  color: inherit;
  align-items: center;
  justify-content: center;

  padding: 0.5em;
}

.button[checked="true"] {
  background-color: var(--accent-color);
  color: var(--background-color);
}

@media (hover: hover) and (pointer: fine) {
  .button:hover {
    cursor: pointer;
    background-color: var(--text-color);
    color: var(--background-color);
  }
}

.button:active {
  cursor: pointer;
  background-color: var(--accent-color);
  color: var(--background-color);
}
</style>
