# Common imports
import pandas as pd
import numpy as np
from sklearn import preprocessing
# import seaborn as sns
# sns.set(color_codes=True)
import matplotlib.pyplot as plt


path = "C:\\Users\\Amirashkan\\Desktop\\Unity\\Shared Project\\Backup\\02142022\\DT_WindFarm_MasterThesis\\Assets\\CSV\\"
merged_data = pd.read_csv(path + "Saved_data - Backup.csv")

print(f'Total Data Points {merged_data.shape[0] + 1}')

data = merged_data.copy() 
# data[data.columns[0]] = pd.to_datetime(data[data.columns[0]], format='%Y-%m-%d')
data[data.columns[0]] = pd.to_datetime(data[data.columns[0]])
data = data.set_index(data.columns[0])
data = data.sort_index()

value = 'Vibr1'
indx1 = 0
indx2 = 1
graphName = 'Turbine 1'
slices = round(len(data) * 0.05)


# mean = data.groupby(np.arange(len(data))//slices).mean()
DateTime = data.iloc[slices::slices , :0]
# std = data.groupby(np.arange(len(data))//slices).std()
# skew = data.groupby(np.arange(len(data))//slices).skew()
# kurt = data.groupby(np.arange(len(data))//slices).apply(pd.DataFrame.kurt)
def rms(values):
    return np.sqrt(sum(values**2)/len(values))

rms1 = data.groupby(np.arange(len(data))//slices)[value].apply(rms)


df = pd.DataFrame()
df[value] = rms1[:len(DateTime)]
df.index = DateTime.index
length = len(df)
minRange = int(length * 5/100)
maxRange = int(length * 90/100)
finalRange = int(length * 97/100)

healthy_data = df[df.index[0]:df.index[maxRange]]

max_Threshold = max(healthy_data.Vibr1)
min_Threshold = min(healthy_data.Vibr1)
healthy_data
df["max_Threshold"] = max_Threshold
df["min_Threshold"] = min_Threshold




x = pd.read_csv(path + "rsmBoundary.csv")
df2 = pd.DataFrame(x);
df2.drop(df2.columns[[indx1,indx2]], inplace=True, axis=1)
df2.insert(loc= indx1, column='min_Threshold'+ str(indx1), value= min_Threshold)
df2.insert(loc= indx2, column='max_Threshold'+ str(indx1), value= max_Threshold)
df2.to_csv(path + 'rsmBoundary.csv',index=False)


df.plot(logy=False,  figsize = (16,6), style=['ko-','r-.','b-.'],linewidth=2)
plt.fill_between(df.index,max_Threshold, min_Threshold,color = 'C9',alpha=0.2)
plt.xlabel("Time",size=15)
plt.ylabel("RMS",size=15)
plt.title(graphName,size=15)
plt.show()
#Chossing data for training
train_data = healthy_data

# Creating training dataframe
prophet_healthy_train = pd.DataFrame()
prophet_healthy_train['ds'] = train_data.index
prophet_healthy_train['y'] = train_data.values


# prophet_healthy_train2.head()
from fbprophet import Prophet

m = Prophet(yearly_seasonality=False, weekly_seasonality=False,daily_seasonality=False, mcmc_samples=0, growth='linear', interval_width= 1)
    # Using the training data from "healthy part"
m.add_seasonality(name='Secondly', period= 1,fourier_order= 60 , mode ='multiplicative')
# m = Prophet(mcmc_samples=0, growth='linear', interval_width= 1)
# Using the training data from "healthy part"
# m.add_seasonality(name='Hourly', period= 1,fourier_order= 1 , mode ='multiplicative') 
# Using the training data from "healthy part"
# m.add_seasonality(name='Hourly', period= 1,fourier_order= 24 , mode ='multiplicative') 
# m.add_seasonality(name='Secondly', period= 1,fourier_order= 60 , mode ='multiplicative') 
m.fit(prophet_healthy_train)

forecast = m.predict(prophet_healthy_train)

# test_data = df[df.index[maxRange]:df.index[finalRange]][value]
# # prophet_faultydata.head()
# prophet_faulty_test = pd.DataFrame()
# prophet_faulty_test['ds'] = test_data.index
# prophet_faulty_test['y'] = test_data.values
ftr = int((length * 70) / 100)*20
future_data = m.make_future_dataframe(periods = ftr, freq = 's',include_history = True)
forecast = m.predict(future_data)
# m.plot_components(forecast)
# forecast['fact'] = prophet_faulty_test['y'].reset_index(drop = True)
# print('Displaying Prophet plot')


fig = m.plot(forecast,figsize=(16, 6))
# plt.scatter(test_data.index, test_data.values, c = 'r', s= 10.0, alpha=0.7)
plt.legend(["Actual/Trained Data","Expected/Predicted",'Acceptable variance','Actual/Test Data'] , fontsize=15)
plt.xlabel("Time",size=15)
plt.ylabel("RMS",size=15)
plt.title(graphName,size=15)
plt.show()


#Vibration prediction
length = len(data)
minRange = int(length * 5/100)
maxRange = int(length * 80/100)


#Chossing data for training
healthy_bearing1 = data[data.index[0]:data.index[maxRange]][value]

# Creating training dataframe
prophet_healthy_train = pd.DataFrame()
prophet_healthy_train['ds'] = healthy_bearing1.index
prophet_healthy_train['y'] = healthy_bearing1.values

#Vibration Prediction
from fbprophet import Prophet
m2 = Prophet(yearly_seasonality=False, weekly_seasonality=False,daily_seasonality=False, mcmc_samples=0, growth='linear', interval_width= 1)
# Using the training data from "healthy part"
m2.add_seasonality(name='Secondly', period= 1,fourier_order= 1 , mode ='multiplicative') 
m2.fit(prophet_healthy_train)


forecast = m2.predict(prophet_healthy_train)
prophet_faultydata = data[data.index[maxRange]:data.index[-1]][value]
# prophet_faultydata.head()
prophet_faulty_test = pd.DataFrame()
prophet_faulty_test['ds'] = prophet_faultydata.index
prophet_faulty_test['y'] = prophet_faultydata.values
ftr = int((length * 70) / 100)
future_data = m2.make_future_dataframe(periods = ftr, freq = 's')
forecast = m2.predict(future_data)
# m.plot_components(forecast)
forecast['fact'] = prophet_faulty_test['y'].reset_index(drop = True)
# print('Displaying Prophet plot')
fig = m2.plot(forecast,figsize=(16, 3))
plt.scatter(prophet_faultydata.index, prophet_faultydata.values, c = 'r', s= 10.0, alpha=0.7)
plt.legend(["Actual/Trained Data","Expected/Predicted",'Acceptable variance','Actual/Test Data'] , fontsize=15)
plt.xlabel("Date Time",size=15)
plt.ylabel("Vibration",size=15)
plt.title(graphName,size=15)
plt.show()




