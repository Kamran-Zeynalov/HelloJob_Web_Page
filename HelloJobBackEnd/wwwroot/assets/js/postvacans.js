window.addEventListener('load', function() {
    var work_pls = document.querySelector('.work_pls');
    var work_mns = document.querySelector('.work_mns');
    var work_add = document.querySelector('.work_add');
    var hidden_work = document.querySelector('.hidden_work');
  
    work_pls.addEventListener('click', function(e) {
        e.preventDefault();
      var addInput = work_add.querySelector('.add_input');
      var newInputValue = addInput.value.trim();
      if (newInputValue !== '') {
        var currentValue = hidden_work.value.trim();
        var separator = currentValue === '' ? '' : '/';
        hidden_work.value = currentValue + separator + newInputValue;
        addInput.value = '';
      }
    });

    work_mns.addEventListener('click', function(e) {
        e.preventDefault();
        var currentValue = hidden_work.value.trim();
        if (currentValue !== '') {
          var values = currentValue.split('/');
          var newValues = values.slice(0, -1);
          hidden_work.value = newValues.join('/');
        }
      });




      var user_pls = document.querySelector('.user_pls');
      var user_mns = document.querySelector('.user_mns');
      var user_add = document.querySelector('.user_add');
      var hidden_user = document.querySelector('.hidden_user');
    
      user_pls.addEventListener('click', function(e) {
          e.preventDefault();
        var addInput = user_add.querySelector('.add_input_user');
        var newInputValue = addInput.value.trim();
        if (newInputValue !== '') {
          var currentValue = hidden_user.value.trim();
          var separator = currentValue === '' ? '' : '/';
          hidden_user.value = currentValue + separator + newInputValue;
          addInput.value = '';
        }
      });
  
      user_mns.addEventListener('click', function(e) {
          e.preventDefault();
          var currentValue = hidden_user.value.trim();
          if (currentValue !== '') {
            var values = currentValue.split('/');
            var newValues = values.slice(0, -1);
            hidden_user.value = newValues.join('/');
          }
        });
  });