mergeInto(LibraryManager.library, {
  TestEvent: function (userName, score) {
    window.dispatchReactUnityEvent("TestEvent", UTF8ToString(userName), score);
  },
});